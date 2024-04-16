using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Classes
{
    public class AuthUserDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public bool KeepSigned { get; set; }
	}
}
