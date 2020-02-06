using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bien.Core.Models
{
    public class DepartmentVentilation : EntityBase
    {
        [Required, StringLength(500)]
        public string UnitName { get; set; }

        public int DepartmentUid { get; set; }

        public string Departmentkey { get; set; }

        public string Capacity { get; set; }
    }
}