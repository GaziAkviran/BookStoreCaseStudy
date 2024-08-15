using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PointBooks.Models;
using PointBooks.Repositories;

namespace PointBooks.Controllers.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly IGenericRepository<Order> _orderRepository;

        public OrderApiController(IGenericRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepository.GetAll();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderByID(int id)
        {
            var orders = await _orderRepository.GetByID(id);
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }

            await _orderRepository.Create(order);
            return CreatedAtAction(nameof(GetOrderByID), new { id = order.OrderID }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            if (order == null || order.OrderID != id)
            {
                return BadRequest();
            }

            var existingOrder = await _orderRepository.GetByID(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            await _orderRepository.Update(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetByID(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderRepository.Delete(id);
            return NoContent();
        }
    }
}
