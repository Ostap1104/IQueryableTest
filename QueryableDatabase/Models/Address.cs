using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryableDatabase.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }
    }
}
