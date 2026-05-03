using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BussinesLogic.DTOs.CartItemsDTOs
{
    public class CartItemsUpdateDTO
    {
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
