using System.ComponentModel.DataAnnotations;

namespace webapi.Requests.User
{
    public class UserCreateRequest
    {
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

        public IFormFile? Image { get; set; }
    }
}
