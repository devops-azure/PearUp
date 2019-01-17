using System.ComponentModel.DataAnnotations;

namespace PearUp.DTO
{
    public abstract class PasswordDTO
    {
        [Required]
        public string Password { get; set; }

    }
}
