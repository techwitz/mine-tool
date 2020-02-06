using System.Collections.Generic;
using System.Threading.Tasks;
using Bien.Core.Models;
using Bien.Core.Services;
using Dapper;
using System;
using System.Data;
using System.Linq;
using Bien.Core.Types;

namespace Bien.DataAcess.Stores
{
    public class DbDepartmentStore : DbStoreBase<Department>, IDepartmentStore
    {
        private const string TableName = "[dbo].[Department]";

        public DbDepartmentStore(IDbFactory factory)
            : base(factory)
        {
        }

        public async Task<IList<Department>> GetAllAsync()
        {
            var sql = $@"SELECT [Uid], [EntityKey], [Name], [Capacity], [Created], [RowStamp] FROM {TableName} ORDER BY Name";
            var results = await Db.QueryAsync<Department>(sql);
            return results.ToList();
        }

        public async Task<StoreResult> InsertRecord(DepartmentVentilation model)
        {
            StoreResult result = StoreResult.Success;
            using (var txn = Db.BeginTransaction(IsolationLevel.RepeatableRead))
            {
                try
                {
                    var param = new DynamicParameters(model)
                        .Output(model, m => m.Uid)
                        .Output(model, m => m.RowStamp)
                        .Output(model, m => m.Created);

                    var sql = $@"
                                Select @DepartmentUid = Uid from Department where EntityKey = @DepartmentKey;

                                INSERT INTO [dbo].[VentilationCapacity]
                                ([EntityKey],[UnitName],[DepartmentUid],[Capacity],[Created])
                                VALUES
                                (@EntityKey, @UnitName, @DepartmentUid, @Capacity, SYSUTCDATETIME());

                                SELECT @Uid = [Uid], @Created = Created, @RowStamp = RowStamp FROM [dbo].[VentilationCapacity] WHERE [Uid] = SCOPE_IDENTITY();  ";

                    var results = await Db.ExecuteAsync(sql, param, txn);

                    if (results != -1)
                    {
                        txn.Commit();
                    }
                    else
                    {
                        txn.Rollback();
                        result = StoreResult.Failure();
                    }
                }
                catch (Exception ex)
                {
                    txn.Rollback();
                    result = StoreResult.DbException(ex);
                }
            }

            return result;
        }
    }
}