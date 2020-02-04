using Bien.Core.Types;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Bien.DataAcess
{
    public static class DbExtensions
    {
        /*
        KW: the syntax in this method is specific to SQL Server; whilst it's unlikely that
        we'll suddenly move to MySQL, this is technically a violation of platform independence
        and, at some point, we should consider removing it in favour of an injectable
        database-specific provider. It's not important right now, though.
        */

        public static async Task<PagedDataSet<TModel>> GetPagedDataAsync<TModel>(this IDbConnection db, string sql, object param, int page, int rowsPerPage, string orderBy, IDbTransaction txn = null)
        {
            var dparam = (param as DynamicParameters) ?? new DynamicParameters(param);
            dparam.Add("Offset", (page - 1) * rowsPerPage);
            dparam.Add("PageSize", rowsPerPage);

            // provides a wrapper SQL statement to count the number of rows returned
            // by the inner SQL statement
            var countWrapper = $@"
                                WITH results AS ({sql})
                                SELECT COUNT(*) FROM results;";

            // provides a wrapper SQL statement to return the desired page of results
            // from the inner SQL statement
            var resultsWrapper = $@"
                        WITH results AS ({sql})
                        SELECT * FROM results
                        ORDER BY {orderBy}
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            int count;
            IEnumerable<TModel> results;

            // use a transaction to prevent dirty reads between counting and
            // fetching rows (we don't want a mismatch or we'll get bug reports)

            // users have the option of passing in an existing transaction; if
            // so, we do not commit or dispose of it, but merely enlist in it;
            // otherwise, we create our own transaction and commit/dispose it
            // internally.
            var internalTxn = txn == null;
            if (internalTxn)
            {
                if (db.State != ConnectionState.Open) db.Open();
                txn = db.BeginTransaction(IsolationLevel.ReadUncommitted);
                internalTxn = true;
            }

            try
            {
                if (db.State != ConnectionState.Open) db.Open();
                count = (await db.ExecuteScalarAsync(countWrapper, dparam, txn) as int?) ?? 0;
                results = await db.QueryAsync<TModel>(resultsWrapper, dparam, txn);

                // commit internal transactions
                if (internalTxn)
                {
                    txn.Commit();
                }

                return new PagedDataSet<TModel>(results.ToList(), page, rowsPerPage, count);
            }
            catch
            {
                // rollback internal transactions
                if (internalTxn)
                {
                    txn.Rollback();
                }

                throw;
            }
            finally
            {
                // dispose internal transactions
                if (internalTxn)
                {
                    txn.Dispose();
                }
            }
        }

        public static async Task<StoreResult> DeleteAsync(this IDbConnection db, string table, string where, object param, byte[] rowStamp = null, IDbTransaction txn = null, string rowStampFieldName = "RowStamp")
        {
            var builder = new SqlBuilder();

            // enable NOCOUNT and take care of returning the rowcount ourselves; this means
            // that we know that the result of this query will be the number of rows affected;
            // we can then assume that >=1 means that our deletion was successful, and 0 means
            // that it wasn't (might end up with >1 if there are cascading FKs or triggers)
            var template = builder.AddTemplate($@"
SET NOCOUNT ON;
DELETE FROM [{DbUtil.CleanseFieldName(table)}] /**where**/;
SET @ResultCount = @@ROWCOUNT");

            // append the core where clause
            builder.Where(where, param);

            var dynamicParams = new DynamicParameters(param);
            dynamicParams.Add("ResultCount", 0, DbType.Int32, ParameterDirection.Output);

            if (rowStamp != null)
            {
                // if we've provided a concurrency token/rowstamp, then use it; otherwise we
                // apparently don't care and just want to nuke the data from orbit
                builder.Where($"[{DbUtil.CleanseFieldName(rowStampFieldName)}] = @rowStamp", rowStamp);
                dynamicParams.Add("rowStamp", rowStamp);
            }

            try
            {
                await db.ExecuteAsync(template.RawSql, dynamicParams, txn);
                var result = dynamicParams.Get<int>("ResultCount");

                if (result == 1)
                {
                    // woohoo!
                    return StoreResult.Success;
                }
                else if (rowStamp != null)
                {
                    // if we supplied a rowstamp, then returning 0 rows probably means that someone
                    // else changed the data, and we need to either return an error or re-run the
                    // txn - either way, this problem is above our pay-grade
                    return StoreResult.ConcurrencyError();
                }
                else
                {
                    // generic failure of genericness
                    return StoreResult.Failure(StoreResultErrorCodes.NotFound);
                }
            }
            catch (DbException ex)
            {
                return StoreResult.DbException(ex);
            }
            catch (Exception ex)
            {
                return StoreResult.UnhandledException(ex);
            }
        }

        public static async Task<bool> IsValidSortingColumn(this IDbConnection db, string tableName, string columnName)
        {
            var sql = @"
SELECT TOP 1 c.Name
FROM sys.columns AS c
JOIN sys.tables AS t ON c.object_id = t.object_id
WHERE t.name = @tableName
AND c.name = @columnName";

            var result = await db.ExecuteScalarAsync<string>(sql, new { tableName, columnName });
            return result != null;
        }
    }
}