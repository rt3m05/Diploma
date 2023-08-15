using System.ComponentModel.DataAnnotations;
using System.Data;

namespace webapi.Requests.User
{
    public class UserCreateRequest
    {
        [Required]
        public string? Nickname { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }

        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }

        public byte[]? Image { get; set; }
    }
}
