using SmartShoppingAssistant.BussinesLogic.DTOs.CartItemsDTOs;
using SmartShoppingAssistant.BussinesLogic.Models;

namespace SmartShoppingAssistant.BussinesLogic.Services.Interfaces
{
    public interface ICartItemsService
    {
        Task<CartSummaryDTO> GetCurrentCart();
        Task<CartSummaryDTO> AddCartItemAsync(CartItemsCreateDTO cartItemsCreateDTO);
        Task<CartSummaryDTO> UpdateCartItemAsync(int id, CartItemsUpdateDTO cartItemsUpdateDTO);
        Task DeleteCartItemAsync(int id);
        Task ClearCartAsync();
        Task<AnalysisResponse> AnalyzeCartAsync();
    }
}
