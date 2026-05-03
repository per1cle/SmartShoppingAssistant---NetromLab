using SmartShoppingAssistant.BussinesLogic.DTOs.CartItemsDTOs;
using SmartShoppingAssistant.BussinesLogic.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BussinesLogic.Services.Interfaces
{
    public interface ICartItemsService
    {
        Task<CartSummaryDTO> GetCurrentCart();
        Task<CartItemsGetDTO> AddCartItemAsync(CartItemsCreateDTO cartItemsCreateDTO);
        Task<CartItemsGetDTO> UpdateCartItemAsync(int id, CartItemsUpdateDTO cartItemsUpdateDTO);
        Task DeleteCartItemAsync(int id);
        Task ClearCartAsync();
    }
}
