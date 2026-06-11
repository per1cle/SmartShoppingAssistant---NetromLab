using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BussinesLogic.DTOs.CartItemsDTOs
{
    public class CartItemsGetDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
