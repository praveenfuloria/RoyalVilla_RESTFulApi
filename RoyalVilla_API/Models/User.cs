using System.ComponentModel.DataAnnotations;

namespace RoyalVilla_API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [MaxLength(60)]
        public string Role { get; set; } = "Customer";

        public DateTime CreatedDate { get; set; } 

        public DateTime UpdatedDate { get; set; } 
    }
}
