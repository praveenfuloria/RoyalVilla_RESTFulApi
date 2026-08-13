using System.ComponentModel.DataAnnotations;

namespace RoyalVilla_API.Models.DTO
{
    public class VillaAmentiesCreateDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public int VillaId { get; set; }

    }
}
