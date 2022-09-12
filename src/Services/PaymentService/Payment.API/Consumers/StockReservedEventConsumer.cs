using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        readonly IPublishEndpoint _publishEndpoint;
        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            bool paymentState = true;
            if (paymentState)
            {
                Console.WriteLine("Payment Successed...");
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
                };
                await _publishEndpoint.Publish(paymentCompletedEvent);
            }
            else
            {
                Console.WriteLine("Payment Failed...");
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems,
                    Message = "Insufficient balance!"
                };
                await _publishEndpoint.Publish(paymentFailedEvent);
            }
        }
    }
}
