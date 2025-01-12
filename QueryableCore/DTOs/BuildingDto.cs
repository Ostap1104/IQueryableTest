using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryableCore.DTOs
{
    public class BuildingDto
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressDto Address{get; set;}
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public int Floors { get; set; }
        private int YearBuilt { get; set; }
    }
}
