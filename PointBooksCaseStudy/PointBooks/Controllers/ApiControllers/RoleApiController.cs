using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Repositories;

namespace PointBooks.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleApiController : ControllerBase
    {
        private readonly IGenericRepository<Role> _roleRepository;

        public RoleApiController(IGenericRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleRepository.GetAll();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleByID(int id)
        {
            var role = await _roleRepository.GetByID(id);
            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] Role role)
        {
            if (role == null)
            {
                return BadRequest();
            }

            await _roleRepository.Create(role);
            return CreatedAtAction(nameof(GetRoleByID), new { id = role.RoleID }, role);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] Role role)
        {
            if (role == null || role.RoleID != id)
            {
                return BadRequest();
            }

            var existingRole = await _roleRepository.GetByID(id);
            if (existingRole == null)
            {
                return NotFound();
            }

            await _roleRepository.Update(role);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _roleRepository.GetByID(id);
            if (role == null)
            {
                return NotFound();
            }

            await _roleRepository.Delete(id);
            return NoContent();
        }
    }
}
