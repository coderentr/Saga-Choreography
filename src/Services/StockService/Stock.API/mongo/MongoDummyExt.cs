using MongoDB.Driver;
namespace Stock.API.mongo
{
    public static class MongoDummyExt
    {
        public static void CreateDummyData(this WebApplication app)
        {
            var mongoDbService = app.Services.GetService<MongoDbService>();
            if (!mongoDbService.GetCollection<Models.Entities.Stock>().FindSync(x => true).Any())
            {
                mongoDbService.GetCollection<Models.Entities.Stock>().InsertOne(new()
                {
                    ProductId = 21,
                    Count = 200
                });
                mongoDbService.GetCollection<Models.Entities.Stock>().InsertOne(new()
                {
                    ProductId = 22,
                    Count = 100
                });
                mongoDbService.GetCollection<Models.Entities.Stock>().InsertOne(new()
                {
                    ProductId = 23,
                    Count = 50
                });
                mongoDbService.GetCollection<Models.Entities.Stock>().InsertOne(new()
                {
                    ProductId = 24,
                    Count = 10
                });
                mongoDbService.GetCollection<Models.Entities.Stock>().InsertOne(new()
                {
                    ProductId = 25,
                    Count = 30
                });
            }
        }
    }
}
