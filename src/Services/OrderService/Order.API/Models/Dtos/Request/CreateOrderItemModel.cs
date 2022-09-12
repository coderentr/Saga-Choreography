namespace Order.API.Models.Dtos.Request
{
    public class CreateOrderItemModel
    {
        public int Count { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
    }
}
