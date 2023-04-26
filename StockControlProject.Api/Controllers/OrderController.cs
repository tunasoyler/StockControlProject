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

        public OrderController(IGenericService<Order> orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            return Ok(orderService.GetAll(x=>x.User, x=>x.OrderDetails));
        }
    }
}
