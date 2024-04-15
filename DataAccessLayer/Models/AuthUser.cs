using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class AuthUser
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool Remember { get; set; }
    }
}
