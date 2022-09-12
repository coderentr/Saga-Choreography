using MassTransit;
using MongoDB.Driver;
using Shared.Events;
using Shared.RabbitMQ;
using Stock.API.mongo;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        readonly MongoDbService _mongoDbService;
        readonly ISendEndpointProvider _sendEndpointProvider;
        readonly IPublishEndpoint _publishEndpoint;
        public OrderCreatedEventConsumer(MongoDbService mongoDbService,
       ISendEndpointProvider sendEndpointProvider,
       IPublishEndpoint publishEndpoint)
        {
            _mongoDbService = mongoDbService;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            IMongoCollection<Models.Entities.Stock> collection =
                _mongoDbService.GetCollection<Models.Entities.Stock>();
            foreach (OrderItemMessage orderItem in context.Message.OrderItems)
                stockResult.Add((await collection.FindAsync(s => s.ProductId == orderItem.ProductId && s.Count > orderItem.Count)).Any());

            if (stockResult.TrueForAll(sr => sr.Equals(true)))
            {
                foreach (OrderItemMessage orderItem in context.Message.OrderItems)
                {
                    Models.Entities.Stock stock = await (await collection.FindAsync(s => s.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();
                    stock.Count -= orderItem.Count;
                    await collection.FindOneAndReplaceAsync(x => x.ProductId == orderItem.ProductId, stock);
                }
                ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(
                    new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
                StockReservedEvent stockReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems,
                    TotalPrice = context.Message.TotalPrice
                };
                await sendEndpoint.Send(stockReservedEvent);
            }
            else
            {
                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "Insufficient stock...."
                };
                await _publishEndpoint.Publish(stockNotReservedEvent);
            }
        }
    }
}
