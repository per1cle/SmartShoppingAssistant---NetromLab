using SmartShoppingAssistant.BussinesLogic.DTOs.PromotionDTOs;

namespace SmartShoppingAssistant.BussinesLogic.DTOs.CartItemsDTOs
{
    public class CartSummaryDTO
    {
        public List<CartItemsSummaryDTO> Items { get; set; } = new();
        public decimal Subtotal { get; set; } 
        public List<AppliedPromotionDTO> AppliedPromotions{ get; set; } = new();    
        public decimal TotalDiscount { get; set; } 
        public decimal Total { get; set; } 
    }
}
