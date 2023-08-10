using System.Data;
using System.Text.Json.Serialization;

namespace webapi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public byte[]? Image { get; set; }
    }
}
