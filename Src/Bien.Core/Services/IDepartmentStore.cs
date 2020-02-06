using Bien.Core.Models;
using Bien.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bien.Core.Services
{
    public interface IDepartmentStore
    {
        Task<IList<Department>> GetAllAsync();

        Task<StoreResult> InsertRecord(DepartmentVentilation model);
    }
}