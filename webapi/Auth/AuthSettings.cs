using Microsoft.IdentityModel.Tokens;
using System.Text;
using webapi.Exceptions;

namespace webapi.Auth
{
    public class AuthSettings
    {
        public string ISSUER { get; set; }
        public string AUDIENCE { get; set; }
        public string KEY { get => null; set => _key = value; }
        string? _key { get; set; }
        public int LIFETIME { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            if (_key == null)
                throw new NullVariableException("AUTH ERROR: Key was null");

            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}
