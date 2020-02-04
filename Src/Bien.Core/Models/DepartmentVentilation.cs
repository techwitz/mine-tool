using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Bien.Core.Models
{
    public class DepartmentVentilation : EntityBase
    {
        [Required, StringLength(500)]
        public string Unit { get; set; }

        public ICollection<VentilationInfo> Ventilations { get; set; } = new List<VentilationInfo>();
    }

    public class VentilationInfo
    {
        public int DepartmentId { get; set; }

        public int VentilationCapacity { get; set; }
    }
}