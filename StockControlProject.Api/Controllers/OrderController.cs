using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockControlProject.Entities.Entities;
using StockControlProject.Service.Abstract;

namespace StockControlProject.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> orderService;
        private readonly IGenericService<OrderDetails> orderDetailsService;

        public OrderController(IGenericService<Order> orderService, IGenericService<OrderDetails> orderDetailsService)
        {
            this.orderService = orderService;
            this.orderDetailsService = orderDetailsService;
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok(orderService.GetAll(x => x.User, x => x.OrderDetails));
        }

        [HttpGet]
        public IActionResult GetActiveOrders()
        {
            return Ok(orderService.GetActive());
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            return Ok(orderService.GetById(id, x => x.OrderDetails));
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderDetailsById(int id)
        {
            return Ok(orderDetailsService.GetAll(x => x.OrderId == id, x => x.Product));
        }

    }
}
