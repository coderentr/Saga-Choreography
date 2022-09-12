using MassTransit;
using Shared.RabbitMQ;
using Stock.API.Consumers;
using Stock.API.mongo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<OrderCreatedEventConsumer>();
    configure.AddConsumer<PaymentFailedEventConsumer>();
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
        configurator.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e =>
        e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
        configurator.ReceiveEndpoint(RabbitMQSettings.Stock_PaymentFailedEventQueue, e => 
        e.ConfigureConsumer<PaymentFailedEventConsumer>(context));
    });
});
builder.Services.AddSingleton<MongoDbService>();



var app = builder.Build();

app.CreateDummyData();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
