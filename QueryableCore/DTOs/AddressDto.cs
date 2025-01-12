using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryableCore.DTOs
{
    public class AddressDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }
    }
}
