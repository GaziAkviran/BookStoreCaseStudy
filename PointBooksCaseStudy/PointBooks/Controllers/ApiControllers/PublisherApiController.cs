using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Repositories;

namespace PointBooks.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherApiController : ControllerBase
    {
        private readonly IGenericRepository<Publisher> _publisherRepository;

        public PublisherApiController(IGenericRepository<Publisher> publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublishers()
        {
            var publishers = await _publisherRepository.GetAll();
            return Ok(publishers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPublisherByID(int id)
        {
            var publisher = await _publisherRepository.GetByID(id);
            if (publisher == null)
            {
                return NotFound();
            }

            return Ok(publisher);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePublisher([FromBody] Publisher publisher)
        {
            if (publisher == null)
            {
                return BadRequest();
            }

            await _publisherRepository.Create(publisher);
            return CreatedAtAction(nameof(GetPublisherByID), new { id = publisher.PublisherID }, publisher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublisher(int id, [FromBody] Publisher publisher)
        {
            if (publisher == null || publisher.PublisherID != id)
            {
                return BadRequest();
            }

            var existingPublisher = await _publisherRepository.GetByID(id);
            if (existingPublisher == null)
            {
                return NotFound();
            }

            await _publisherRepository.Update(publisher);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _publisherRepository.GetByID(id);
            if (publisher == null)
            {
                return NotFound();
            }

            await _publisherRepository.Delete(id);
            return NoContent();
        }
    }
}
