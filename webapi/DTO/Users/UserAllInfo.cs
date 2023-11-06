using webapi.Models;

namespace webapi.DTO.Users
{
    public class UserAllInfo
    {
        public string? Nickname { get; set; }
        public string? Email { get; set; }
        public byte[]? Image { get; set; }

        public UserAllInfo() { }

        public UserAllInfo(User user)
        {
            Nickname = user.Nickname;
            Email = user.Email;
            Image = user.Image;
        }
    }
}
