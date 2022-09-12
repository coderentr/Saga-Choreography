using Order.API.Models.Entities;

namespace Order.API.Models.Dtos.Request
{
    public class CreateOrderModel
    {
        public int BuyerId { get; set; }
        public List<CreateOrderItemModel> OrderItems { get; set; }
    }
}
