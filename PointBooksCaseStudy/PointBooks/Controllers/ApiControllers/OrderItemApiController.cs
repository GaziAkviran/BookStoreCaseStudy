using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Repositories;

namespace PointBooks.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemApiController : ControllerBase
    {
        private readonly IGenericRepository<OrderItem> _orderItemRepository;

        public OrderItemApiController(IGenericRepository<OrderItem> orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderItems()
        {
            var orderItems = await _orderItemRepository.GetAll();
            return Ok(orderItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItemsByID(int id)
        {
            var orderItem = await _orderItemRepository.GetByID(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem([FromBody] OrderItem orderItem)
        {
            if (orderItem == null)
            {
                return BadRequest();
            }

            await _orderItemRepository.Create(orderItem);
            return CreatedAtAction(nameof(GetOrderItemsByID), new { id = orderItem.OrderItemID }, orderItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] OrderItem orderItem)
        {
            if (orderItem == null || orderItem.OrderItemID != id)
            {
                return BadRequest();
            }

            var existingOrderItem = await _orderItemRepository.GetByID(id);
            if (existingOrderItem == null)
            {
                return NotFound();
            }

            await _orderItemRepository.Update(orderItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var author = await _orderItemRepository.GetByID(id);
            if (author == null)
            {
                return NotFound();
            }

            await _orderItemRepository.Delete(id);
            return NoContent();
        }
    }
}
