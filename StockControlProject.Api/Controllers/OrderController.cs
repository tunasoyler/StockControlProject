using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockControlProject.Entities.Entities;
using StockControlProject.Entities.Enums;
using StockControlProject.Service.Abstract;

namespace StockControlProject.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IGenericService<Order> orderService;
        private readonly IGenericService<OrderDetails> orderDetailsService;
        private readonly IGenericService<Product> productService;

        public OrderController(IGenericService<Order> orderService, IGenericService<OrderDetails> orderDetailsService, IGenericService<Product> productService)
        {
            this.orderService = orderService;
            this.orderDetailsService = orderDetailsService;
            this.productService = productService;
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

        [HttpGet]
        public IActionResult GetPendingOrders()
        {
            return Ok(orderService.GetDefault(x => x.Status == Status.Pending));
        }

        [HttpGet]
        public IActionResult GetCompletedOrders()
        {
            return Ok(orderService.GetDefault(x => x.Status == Status.Completed));
        }

        [HttpGet]
        public IActionResult GetCancelledOrders()
        {
            return Ok(orderService.GetDefault(x => x.Status == Status.Cancelled));
        }

        [HttpPost]
        public IActionResult CreateOrder(int userId, [FromQuery] int[] productIds, [FromQuery] short[] quantities)
        {
            Order newOrder = new Order();
            newOrder.UserId = userId;
            newOrder.Status = Status.Pending;
            newOrder.State = true; //if order cancelled or completed, state will be true

            orderService.Add(newOrder);
            for (int i = 0; i < productIds.Length; i++)
            {
                OrderDetails newOrderDetail = new OrderDetails();
                newOrderDetail.State = true;
                newOrderDetail.OrderId = newOrder.Id;
                newOrderDetail.ProductId = productIds[i];
                newOrderDetail.Quantity = quantities[i];
                newOrderDetail.UnitPrice = productService.GetById(newOrderDetail.ProductId).UnitPrice;
                orderDetailsService.Add(newOrderDetail);
            }

            return Ok(newOrder);
        }

        [HttpGet("{id}")]
        public IActionResult ConfirmOrder(int id)
        {
            Order order = orderService.GetById(id);

            if (order == null)
                return NotFound();
            else
            {
                List<OrderDetails> orderDetails = orderDetailsService.GetDefault(x => x.OrderId == order.Id).ToList();
                foreach (OrderDetails item in orderDetails)
                {
                    Product productInOrder = productService.GetById(item.ProductId);
                    productInOrder.Stock -= item.Quantity;
                    productService.Update(productInOrder);
                }
                order.Status = Status.Completed;
                order.State = false;

                return Ok(order);
            }
        }

        [HttpGet("{id}")]
        public IActionResult CancelOrder(int id)
        {
            Order order = orderService.GetById(id);

            if (order == null)
                return NotFound();
            else
            {
                order.Status = Status.Cancelled;
                order.State = false;

                orderService.Update(order);
                return Ok(order);
            }
        }

    }
}
