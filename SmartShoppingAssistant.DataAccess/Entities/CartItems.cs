using SmartShoppingAssistant.DataAccess.Entities;
namespace SmartShoppingAssistant.DataAccess.Entities
{
    public class CartItems
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
