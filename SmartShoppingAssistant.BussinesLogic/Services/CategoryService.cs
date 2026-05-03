using SmartShoppingAssistant.BussinesLogic.DTOs.CategoryDTOs;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BussinesLogic.Services
{
    public class CategoryService(IRepository<Category> categoryRepository) : ICategoryService
    {
        public async Task<CategoryGetDTO> GetCategoryByIdAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Category with id {id} not found.");

            return MapToCategoryGetDTO(category);
        }

        public async Task<List<CategoryGetDTO>> GetAllCategoriesAsync()
        {
            var categories = await categoryRepository.GetAllAsync();

            return categories.Select(MapToCategoryGetDTO).ToList();
        }

        public async Task<CategoryGetDTO> AddCategoryAsync(CategoryCreateDTO categoryCreateDTO)
        {
            var category = new Category
            {
                Name = categoryCreateDTO.Name,
                Description = categoryCreateDTO.Description
            };

            var addedCategory = await categoryRepository.AddAsync(category);

            return MapToCategoryGetDTO(addedCategory);
        }

        public async Task<CategoryGetDTO> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryUpdateDTO)
        {
            var existingCategory = await categoryRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Category with id {id} not found.");

            if (categoryUpdateDTO.Name != null)
                existingCategory.Name = categoryUpdateDTO.Name;
            if (categoryUpdateDTO.Description != null)
                existingCategory.Description = categoryUpdateDTO.Description;

            var updatedCategory = await categoryRepository.UpdateAsync(existingCategory);

            return MapToCategoryGetDTO(updatedCategory);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await categoryRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Category with id {id} not found.");
            await categoryRepository.DeleteAsync(category);
        }

        private static CategoryGetDTO MapToCategoryGetDTO(Category category)
        {
            return new CategoryGetDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
