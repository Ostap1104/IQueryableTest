using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models
{
    public class BuildingModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Floors { get; set; }
        [Required]
        public int YearBuilt { get; set; }

    }
}
