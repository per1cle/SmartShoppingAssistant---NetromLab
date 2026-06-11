using SmartShoppingAssistant.BussinesLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BussinesLogic.Services
{
    public class PromotionService(IPromotionRepository promotionRepository, IRepository<Product> productRepository, IRepository<Category> categoryRepository) : IPromotionService
    {
        public async Task<PromotionGetDTO> GetPromotionByIdAsync(int id)
        {
            var promotion = await promotionRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Promotion with id {id} not found.");

            return MapToPromotionGetDTO(promotion);
        }

        public async Task<List<PromotionGetDTO>> GetAllPromotionsAsync()
        {
            var promotions = await promotionRepository.GetAllAsync();

            return promotions.Select(MapToPromotionGetDTO).ToList();
        }

        public async Task<PromotionGetDTO> AddPromotionAsync(PromotionCreateDTO promotionCreateDTO)
        {
            if (promotionCreateDTO.ProductId.HasValue)
            {
                var product = await productRepository.GetByIdAsync(promotionCreateDTO.ProductId.Value);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with id {promotionCreateDTO.ProductId} not found.");
                }
            }

            if (promotionCreateDTO.CategoryId.HasValue)
            {
                var category = await categoryRepository.GetByIdAsync(promotionCreateDTO.CategoryId.Value);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with id {promotionCreateDTO.CategoryId} not found.");
                }
            }

            var promotion = new Promotion       
            {
                Name = promotionCreateDTO.Name,
                Type = promotionCreateDTO.Type,
                Threshold = promotionCreateDTO.Threshold,
                Reward = promotionCreateDTO.Reward,
                RewardValue = promotionCreateDTO.RewardValue,
                ProductId = promotionCreateDTO.ProductId,
                CategoryId = promotionCreateDTO.CategoryId,
                IsActive = promotionCreateDTO.IsActive
            };

            var addedPromotion = await promotionRepository.AddAsync(promotion);

            return MapToPromotionGetDTO(addedPromotion);
        }

        public async Task<PromotionGetDTO> UpdatePromotionAsync(int id, PromotionUpdateDTO promotionUpdateDTO)
        {
            var promotion = await promotionRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Promotion with id {id} not found.");

            if (promotionUpdateDTO.ProductId.HasValue)
            {
                var product = await productRepository.GetByIdAsync(promotionUpdateDTO.ProductId.Value);
                if(product == null)
                    throw new KeyNotFoundException($"Product with id {promotionUpdateDTO.ProductId} not found.");
                promotion.ProductId = promotionUpdateDTO.ProductId ?? promotion.ProductId;
            }

            if(promotionUpdateDTO.CategoryId.HasValue)
            {
                var category = await categoryRepository.GetByIdAsync(promotionUpdateDTO.CategoryId.Value);
                if(category == null)
                    throw new KeyNotFoundException($"Category with id {promotionUpdateDTO.CategoryId} not found.");
                promotion.CategoryId = promotionUpdateDTO.CategoryId ?? promotion.CategoryId;
            }

            promotion.Name = promotionUpdateDTO.Name;
            promotion.Type = promotionUpdateDTO.Type;
            promotion.Threshold = promotionUpdateDTO.Threshold;
            promotion.Reward = promotionUpdateDTO.Reward;
            promotion.RewardValue = promotionUpdateDTO.RewardValue;
            promotion.IsActive = promotionUpdateDTO.IsActive; 

            var updatedPromotion = await promotionRepository.UpdateAsync(promotion);

            return MapToPromotionGetDTO(updatedPromotion);
        }

        public async Task<PromotionGetDTO> UpdatePromotionStatusAsync(int id, bool status)
        {
            var promotion = await promotionRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Promotion with id {id} not found.");
            promotion.IsActive = status;
            var updatedPromotion = await promotionRepository.UpdateAsync(promotion);
            return MapToPromotionGetDTO(updatedPromotion);
        }
        public async Task DeletePromotionAsync(int id)
        {
            var promotion = await promotionRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Promotion with id {id} not found.");
            await promotionRepository.DeleteAsync(promotion);
        }

        public async Task<List<PromotionGetDTO>> GetForProductAsync(int productId)
        {
            var promotions = await promotionRepository.GetForProductAsync(productId);
            return promotions.Select(MapToPromotionGetDTO).ToList();
        }

        private static PromotionGetDTO MapToPromotionGetDTO(Promotion promotion)
        {
            return new PromotionGetDTO
            {
                Id = promotion.Id,
                Name = promotion.Name,
                Type = promotion.Type,  
                Threshold = promotion.Threshold,    
                Reward = promotion.Reward,
                RewardValue = promotion.RewardValue,    
                ProductId = promotion.ProductId,
                CategoryId = promotion.CategoryId,
                IsActive = promotion.IsActive
            };
        }
    }
}
