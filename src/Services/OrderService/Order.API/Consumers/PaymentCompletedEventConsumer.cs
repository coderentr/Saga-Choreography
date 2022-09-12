using MassTransit;
using Order.API.Models;
using Order.API.Models.Enums;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        readonly OrderDBContext _applicationDbContext;

        public PaymentCompletedEventConsumer(OrderDBContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            Models.Entities.Order order = await _applicationDbContext.Orders.FindAsync(
                context.Message.OrderId);
            if (order != null)
            {
                order.OrderStatus = OrderStatus.Completed;
                await _applicationDbContext.SaveChangesAsync();
            }
        }
    }
}
