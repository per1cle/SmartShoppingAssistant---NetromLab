namespace SmartShoppingAssistant.BussinesLogic.DTOs.CartItemsDTOs
{
    public class CartItemsSummaryDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
