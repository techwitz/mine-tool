using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bien.Core.Models;
using Bien.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bien.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IDepartmentStore _departmentStore;

        public DepartmentController(IServiceProvider serviceProvider, ILogger<DepartmentController> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _departmentStore = serviceProvider.GetService<IDepartmentStore>();
        }

        [HttpGet]
        public async Task<IEnumerable<Department>> Get()
        {
            _logger.LogInformation("Fetch department data start");
            var departments = await _departmentStore.GetAllAsync().ConfigureAwait(false);
            return departments;
        }
    }
}