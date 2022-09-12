using MassTransit;
using Order.API.Models;
using Order.API.Models.Enums;
using Shared.Events;

namespace Order.API.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        readonly OrderDBContext _context;
        public StockNotReservedEventConsumer(OrderDBContext context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            Models.Entities.Order order = await _context.FindAsync<Models.Entities.Order>(context.Message.OrderId);
            if (order != null)
            {
                order.OrderStatus = OrderStatus.Fail;
                await _context.SaveChangesAsync();
                Console.WriteLine(context.Message.Message);
            }
        }
    }
}
