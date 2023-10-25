using webapi.Requests.User;

namespace webapi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? ImageName {  get; set; }
        public byte[]? Image { get; set; }

        public User() { }

        public User(UserCreateRequest req)
        {
            Id = Guid.NewGuid();
            Nickname = req.Nickname;
            Email = req.Email;
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password);
        }
    }
}
