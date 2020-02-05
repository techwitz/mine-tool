using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bien.Core.Models;
using Bien.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Bien.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<IndexModel> _logger;
        private readonly IDepartmentStore _departmentStore;

        public IndexModel(IServiceProvider serviceProvider, ILogger<IndexModel> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _departmentStore = serviceProvider.GetService<IDepartmentStore>();
        }

        public void OnGetAsync()
        {
            _logger.LogInformation("Fetch department data start");
        }
    }
}