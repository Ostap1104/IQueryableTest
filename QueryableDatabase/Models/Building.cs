using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueryableDatabase.Models
{
    public class Building
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string City { get; set; }

        //[ForeignKey(nameof(Address))]
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public int Floors { get; set; }
        public int YearBuilt { get; set; }
    }
}
