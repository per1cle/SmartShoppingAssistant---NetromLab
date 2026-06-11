using SmartShoppingAssistant.BussinesLogic.DTOs.ProductDTOs;
using SmartShoppingAssistant.BussinesLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using System.ComponentModel;

namespace SmartShoppingAssistant.BussinesLogic.Tools
{
    public static class ShoppingTools
    {
        [Description("Get all active promotions that apply to a specific product (by product ID or its category).")]
        public static async Task<List<PromotionGetDTO>> GetPromotionForProduct(
            [Description("The product ID to check")] int productId, 
            IPromotionService promotionService)
        {
            return await promotionService.GetForProductAsync(productId);
        }

        [Description("Get available products by category ID to suggest to the user.")]
        public static async Task<List<ProductGetDTO>> GetProductsByCategory(
            [Description("The category ID to check")] int categoryId, 
            IProductService productService)
        {
            return await productService.GetByCategoryAsync(categoryId);
        }
    }
}
