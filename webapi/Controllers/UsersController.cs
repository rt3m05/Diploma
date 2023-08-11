using Microsoft.AspNetCore.Mvc;
using webapi.DB;
using webapi.DB.Services;
using webapi.Exceptions;
using webapi.Models;
using webapi.Requests.User;

namespace webapi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: api/users
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _userService.GetAll();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            return "value";
        }

        // POST api/users
        [HttpPost]
        public async Task<IActionResult> Create(CreateRequest model)
        {
            Guid id = Guid.Empty;

            try
            {
                id = await _userService.Create(model);
            } 
            catch (EmailExistsException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            if(id == Guid.Empty)
            {
                _logger.LogError("After create user Id was empty.");
                return StatusCode(500);
            }

            _logger.LogInformation("User created. Id: " + id.ToString());
            return Ok(new { message = "User created" });
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateRequest model)
        {
            try
            {
                await _userService.Update(id, model);
            } 
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message + " Id: " + id.ToString());
                return BadRequest(ex.Message);
            } 
            catch (EmailExistsException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }

            _logger.LogInformation("User updated. Id: " + id.ToString());
            return Ok(new { message = "User updated" });
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
