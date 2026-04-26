using SmartShoppingAssistant.BussinesLogic.DTOs;

namespace SmartShoppingAssistant.BussinesLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductGetDTO> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<List<ProductGetDTO>> GetAllAsync();
        Task<ProductGetDTO> AddAsync(ProductCreateDTO productCreateDTO);
        Task<ProductGetDTO> UpdateAsync(ProductUpdateDTO productUpdateDTO);
    }
}
