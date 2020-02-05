using System.Collections.Generic;
using System.Threading.Tasks;
using Bien.Core.Models;
using Bien.Core.Services;
using Dapper;
using System;
using System.Data;
using System.Linq;

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
    }
}