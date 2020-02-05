using System;
using System.ComponentModel.DataAnnotations;

namespace Bien.Core.Models
{
    public class Department : EntityBase
    {
        [StringLength(500)]
        public string Name { get; set; }

        [Range(1, 9999, ErrorMessage = "Ventilation Capacity must be between 1 and 9999.")]
        public int Capacity { get; set; }
    }
}