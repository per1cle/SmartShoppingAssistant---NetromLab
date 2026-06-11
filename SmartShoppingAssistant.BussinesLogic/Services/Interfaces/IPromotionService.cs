using SmartShoppingAssistant.BussinesLogic.DTOs.PromotionDTOs;

namespace SmartShoppingAssistant.BussinesLogic.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<PromotionGetDTO> GetPromotionByIdAsync(int id);
        Task DeletePromotionAsync(int id);
        Task<List<PromotionGetDTO>> GetAllPromotionsAsync();
        Task<PromotionGetDTO> AddPromotionAsync(PromotionCreateDTO promotionCreateDTO);
        Task<PromotionGetDTO> UpdatePromotionAsync(int id, PromotionUpdateDTO promotionUpdateDTO);
        Task<List<PromotionGetDTO>> GetForProductAsync(int productId);
        Task<PromotionGetDTO> UpdatePromotionStatusAsync(int id, bool status);
    }
}
