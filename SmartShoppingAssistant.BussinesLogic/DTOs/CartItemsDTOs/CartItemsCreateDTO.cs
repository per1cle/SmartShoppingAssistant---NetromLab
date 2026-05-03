using SmartShoppingAssistant.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BussinesLogic.DTOs.CartItemsDTOs
{
    public class CartItemsCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
