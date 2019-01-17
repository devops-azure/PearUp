using System.ComponentModel.DataAnnotations;

namespace PearUp.DTO
{
    public class AdminCredentialsDTO: PasswordDTO
    {
        [Required]
        public string EmailId { get; set; }
    }
}
