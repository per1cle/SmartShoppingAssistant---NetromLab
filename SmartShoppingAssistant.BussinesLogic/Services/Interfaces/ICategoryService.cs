using SmartShoppingAssistant.BussinesLogic.DTOs.CategoryDTOs;

namespace SmartShoppingAssistant.BussinesLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryGetDTO> GetCategoryByIdAsync(int id);
        Task DeleteCategoryAsync(int id);
        Task<List<CategoryGetDTO>> GetAllCategoriesAsync();
        Task<CategoryGetDTO> AddCategoryAsync(CategoryCreateDTO categoryCreateDTO);
        Task<CategoryGetDTO> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryUpdateDTO);
    }
}
