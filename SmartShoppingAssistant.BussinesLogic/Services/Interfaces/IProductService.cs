using SmartShoppingAssistant.BussinesLogic.DTOs.ProductDTOs;

namespace SmartShoppingAssistant.BussinesLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductGetDTO> GetProductByIdAsync(int id);
        Task DeleteProductAsync(int id);
        Task<List<ProductGetDTO>> GetAllProductsAsync(int? categoryId = null);
        Task<ProductGetDTO> AddProductAsync(ProductCreateDTO productCreateDTO);
        Task<ProductGetDTO> UpdateProductAsync(int id, ProductUpdateDTO productUpdateDTO);
        Task<List<ProductGetDTO>> SearchAsync(string query);
        Task<List<ProductGetDTO>> GetByCategoryAsync(int categoryId);
    }
}
