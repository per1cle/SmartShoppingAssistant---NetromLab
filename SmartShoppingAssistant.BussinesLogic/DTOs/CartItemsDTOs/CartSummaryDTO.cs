namespace SmartShoppingAssistant.BussinesLogic.DTOs.CartItemsDTOs
{
    public class CartSummaryDTO
    {
        public List<CartItemSummaryDTO> Items { get; set; } = new();
        public decimal Subtotal { get; set; } 
        public decimal Discount { get; set; } 
        public decimal FinalTotal { get; set; } 
    }
}
