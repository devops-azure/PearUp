using System.ComponentModel.DataAnnotations;

namespace PearUp.DTO
{
    public class UserCredentialsDTO: PasswordDTO
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}
