using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using webapi.DB.Services;
using webapi.Exceptions;
using webapi.Models;
using webapi.Requests.User;

namespace webapi.Auth
{
    public interface IAuthService
    {
        Task<string> GetToken(UserLoginRequest model);
        ClaimsIdentity GetClaims(User user);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly AuthSettings _authSettings;

        public AuthService(IUserService userService, IOptions<AuthSettings> authSettings)
        {
            _userService = userService;
            _authSettings = authSettings.Value;
        }

        public async Task<string> GetToken(UserLoginRequest model)
        {
            var user = await _userService.GetByEmail(model.Email!);
            if(user == null)
                throw new KeyNotFoundException("User not found");

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                throw new InvalidPasswordException("Invalid password");

            var now = DateTime.UtcNow;
            var claims = GetClaims(user);

            var jwt = new JwtSecurityToken(
                    issuer: _authSettings.ISSUER,
                    audience: _authSettings.AUDIENCE,
                    notBefore: now,
                    claims: claims.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(_authSettings.LIFETIME)),
                    signingCredentials: new SigningCredentials(_authSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public ClaimsIdentity GetClaims(User user)
        {
            if (user == null)
                throw new NullVariableException("AUTH ERROR: user was null.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!)
            };

            if (user.Nickname != null)
                claims.Add(new Claim(ClaimTypes.Name, user.Nickname));

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, 
                "Token", 
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
