using SmartShoppingAssistant.BussinesLogic.DTOs.ProductDTOs;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BussinesLogic.Services
{
    public class ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository) : IProductService
    {
        public async Task<ProductGetDTO> GetProductByIdAsync(int id)
        {
            var product = await productRepository.GetByIdWithCategoriesAsync(id)
                ?? throw new KeyNotFoundException($"Product with id {id} not found.");

            return MapToProductGetDTO(product);
        }

        public async Task<List<ProductGetDTO>> GetAllProductsAsync(int? categoryId = null)
        {
            var products = await productRepository.GetAllAsync(categoryId);
            
            return products.Select(MapToProductGetDTO).ToList();
        }

        public async Task<ProductGetDTO> AddProductAsync(ProductCreateDTO productCreateDTO)
        {
            var product = new Product
            {
                Name = productCreateDTO.Name,
                Description = productCreateDTO.Description,
                ImageUrl = productCreateDTO.ImageUrl,
                Price = productCreateDTO.Price
            };

            foreach(var categoryId in productCreateDTO.CategoryIds)
            {
                var category = await categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with id {categoryId} not found.");
                }
                product.Categories.Add(category);
            }

            var addedProduct = await productRepository.AddAsync(product);
            
            return MapToProductGetDTO(addedProduct);
        }

        public async Task<ProductGetDTO> UpdateProductAsync(int id, ProductUpdateDTO productUpdateDTO)
        {
            var product = await productRepository.GetByIdWithCategoriesAsync(id)
                ?? throw new KeyNotFoundException($"Product with id {id} not found.");

            product.Name = productUpdateDTO.Name;
            product.Description = productUpdateDTO.Description;
            product.ImageUrl = productUpdateDTO.ImageUrl;
            product.Price = productUpdateDTO.Price;

            if (productUpdateDTO.CategoryIds != null)
            {
                var currentCategoryIds = product.Categories.Select(c => c.Id).ToList();

                var toAddCategoriesIds = productUpdateDTO.CategoryIds.Except(currentCategoryIds).ToList();
                var toRemoveCategoriesIds = currentCategoryIds.Except(productUpdateDTO.CategoryIds).ToList();

                var categoriesToRemove = product.Categories.Where(c => toRemoveCategoriesIds.Contains(c.Id)).ToList();

                foreach (var category in categoriesToRemove)
                {
                    product.Categories.Remove(category);      
                }

                if(toAddCategoriesIds.Any())
                {
                    var categoriesToAdd = await categoryRepository.GetByIdsAsync(toAddCategoriesIds);
                    foreach (var category in categoriesToAdd)
                    {
                        product.Categories.Add(category);
                    }
                }
            }

            var updatedProduct = await productRepository.UpdateAsync(product);

            return MapToProductGetDTO(updatedProduct);
        }
        public async Task DeleteProductAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Product with id {id} not found.");
            await productRepository.DeleteAsync(product);
        }

        public async Task<List<ProductGetDTO>> SearchAsync(string query)
        {
            var products = await productRepository.SearchAsync(query);
            return products.Select(MapToProductGetDTO).ToList();
        }

        public async Task<List<ProductGetDTO>> GetByCategoryAsync(int categoryId)
        {
            var products = await productRepository.GetByCategoryAsync(categoryId);
            return products.Select(MapToProductGetDTO).ToList();
        }

        private static ProductGetDTO MapToProductGetDTO(Product product)
        {
            return new ProductGetDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                CategoryIds = product.Categories.Select(c => c.Id).ToList()
            };
        }
    }
}
