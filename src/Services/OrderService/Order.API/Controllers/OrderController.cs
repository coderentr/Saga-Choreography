using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.Models.Dtos.Request;
using Order.API.Models.Entities;
using Order.API.Models.Enums;
using Shared.Events;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDBContext _context;
        readonly IPublishEndpoint _publishEndpoint;
        public OrderController(OrderDBContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint; 
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderModel model)
        {
            Models.Entities.Order order = new Models.Entities.Order
            {
                BuyerId = model.BuyerId,
                OrderItems = model.OrderItems.Select(oi => new OrderItem
                {
                    Count = oi.Count,
                    Price = oi.Price,
                    ProductId = oi.ProductId
                }).ToList(),
                OrderStatus = OrderStatus.Suspend,
                TotalPrice = model.OrderItems.Sum(oi => oi.Count * oi.Price),
                CreatedDate = DateTime.Now
            };
            await _context.AddAsync<Models.Entities.Order>(order);
            await _context.SaveChangesAsync();
            OrderCreatedEvent orderCreatedEvent = new()
            {
                OrderId = order.Id,
                BuyerId = order.BuyerId,
                TotalPrice = order.TotalPrice,
                OrderItems = order.OrderItems.Select(oi => new OrderItemMessage
                {
                    Price = oi.Price,
                    Count = oi.Count,
                    ProductId = oi.ProductId
                }).ToList()
            };
            await _publishEndpoint.Publish(orderCreatedEvent);
            return Ok(true);
        }
    }
}
