using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using webapi.Auth;
using webapi.DB.Services;
using webapi.DTO.Users;
using webapi.Exceptions;
using webapi.Requests.User;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IAuthService _authService;

        public UsersController(IUserService userService, ILogger<UsersController> logger, IWebHostEnvironment env, IAuthService authService)
        {
            _userService = userService;
            _logger = logger;
            _env = env;
            _authService = authService;
        }

        // GET: api/users/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            if (_env.IsDevelopment())
                try
                {
                    return Ok(await _userService.GetAll());
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500);
                }
            else
                return StatusCode(404);
        }

        // GET api/users
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            string? email = null;
            try
            {
                email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                var user = await _userService.GetByEmailWithImage(email);

                return Ok(new UserAllInfo(user));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Email: " + email);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest model)
        {
            try
            {
                var token = await _authService.GetToken(model);
                
                _logger.LogInformation("User authenticated. Email: " + model.Email);
                return Ok(token);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Email: " + model.Email);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        // POST api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateRequest model)
        {
            Guid id = Guid.Empty;
            string? token = null;

            try
            {
                id = await _userService.Create(model);
                token = await _authService.GetToken(new UserLoginRequest() { Email = model.Email, Password = model.Password });

                if (id == Guid.Empty)
                    throw new Exception("After create user Id was empty.");

                if (string.IsNullOrEmpty(token))
                    throw new Exception("Token was empty.");
            } 
            catch (EmailExistsException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " (After create try to Auth) Email: " + model.Email);
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidPasswordException ex)
            {
                _logger.LogError(ex.Message + " (After create try to Auth)");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            _logger.LogInformation("User created. Id: " + id.ToString());
            return Ok(token);
        }

        // PUT api/users/5
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UserUpdateRequest model)
        {
            string? email = null;
            try
            {
                email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                await _userService.Update(email, model);
            } 
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Email: " + email);
                return BadRequest(new { message = ex.Message });
            } 
            catch (EmailExistsException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (EmptyModelException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            if(model.Email == null)
                _logger.LogInformation("User updated. Email: " + email);
            else
                _logger.LogInformation("User updated. Old Email: " + email + " New Email: " +  model.Email);
            return Ok(new { message = "User updated" });
        }

        // DELETE api/users
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            string? email = null;
            try
            {
                email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
                if (email == null)
                    throw new Exception("Email was null.");

                await _userService.Delete(email);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Email: " + email);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            _logger.LogInformation("User deleted. Email: " + email);
            return Ok(new { message = "User deleted" });
        }
    }
}