using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Repositories;
using PointBooks.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PointBooks.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserApiController(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepository.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByID(int id)
        {
            var user = await _userRepository.GetByID(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            await _userRepository.Create(user);
            return CreatedAtAction(nameof(GetUserByID), new { id = user.UserID }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null || user.UserID != id)
            {
                return BadRequest();
            }

            var existingUser = await _userRepository.GetByID(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            await _userRepository.Update(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetByID(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.Delete(id);
            return NoContent();
        }
    }
}
