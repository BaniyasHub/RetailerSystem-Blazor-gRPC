using Microsoft.AspNetCore.Mvc;
using Retailer.Interface.Dtos;
using Retailer.Interface.Interfaces.Managers;

namespace Retailer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderController : Controller
    {
        private readonly IOrderManager _orderManager;

        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orderList = await _orderManager.GetAllOrders();

            if (orderList != null && orderList.Count() > 0)
            {
                return Ok(orderList);
            }

            return NotFound(new ErrorModelDto()
            {
                ErrorMessage = "There is no product with that id",
                StatusCode = StatusCodes.Status404NotFound
            });
        }

        [HttpGet("orderHeaderId")]
        public async Task<IActionResult> GetOrder(int orderHeaderId)
        {
            var order = await _orderManager.GetOrder(orderHeaderId);

            if (order != null)
            {
                return Ok(order);
            }

            return NotFound(new ErrorModelDto()
            {
                ErrorMessage = "There is no product with that id",
                StatusCode = StatusCodes.Status404NotFound
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderDto orderDto)
        {
            if (orderDto != null)
            {
                var createdOrder = await _orderManager.CreateOrder(orderDto);
                if (createdOrder == null)
                {
                    return BadRequest(new ErrorModelDto
                    {
                        ErrorMessage = "Send the parameters",
                        StatusCode = StatusCodes.Status500InternalServerError
                    });
                }
                return Ok(createdOrder);
            }

            return BadRequest(new ErrorModelDto
            {
                ErrorMessage = "Send the parameters",
                StatusCode = StatusCodes.Status400BadRequest
            });
        }
    }
}
