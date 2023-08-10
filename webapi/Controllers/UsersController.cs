using Microsoft.AspNetCore.Mvc;
using webapi.DB.Services;
using webapi.Models;
using webapi.Requests.User;

namespace webapi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
            await _userService.Create(model);
            return Ok(new { message = "User created" });
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
