using System.ComponentModel.DataAnnotations;
using System.Data;

namespace webapi.Requests.User
{
    public class UserUpdateRequest
    {
        public string? Nickname { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        private string? _password;
        [MinLength(6)]
        public string? Password
        {
            get => _password;
            set => _password = replaceEmptyWithNull(value);
        }

        private string? _confirmPassword;
        [Compare("Password")]
        public string? ConfirmPassword
        {
            get => _confirmPassword;
            set => _confirmPassword = replaceEmptyWithNull(value);
        }

        public byte[]? Image { get; set; }

        private string? replaceEmptyWithNull(string? value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}
