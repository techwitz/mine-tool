using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bien.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Bien.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IEnumerable<Department> Departments { get; set; } = new List<Department>();

        public void OnGet()
        {
        }
    }
}