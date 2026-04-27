using SmartShoppingAssistant.BussinesLogic.DTOs;

namespace SmartShoppingAssistant.BussinesLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductGetDTO> GetProductByIdAsync(int id);
        Task DeleteProductAsync(int id);
        Task<List<ProductGetDTO>> GetAllProductsAsync();
        Task<ProductGetDTO> AddProductAsync(ProductCreateDTO productCreateDTO);
        Task<ProductGetDTO> UpdateProductAsync(int id, ProductUpdateDTO productUpdateDTO);
    }
}
