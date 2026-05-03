using SmartShoppingAssistant.BussinesLogic.DTOs.ProductDTOs;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BussinesLogic.Services
{
    public class ProductService(IProductRepository productRepository, IRepository<Category> categoryRepository) : IProductService
    {
        public async Task<ProductGetDTO> GetProductByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Product with id {id} not found.");

            return MapToProductGetDTO(product);
        }

        public async Task<List<ProductGetDTO>> GetAllProductsAsync(int? categoryId = null)
        {
            var products = await productRepository.GetAllProductsWithCategory(categoryId);
            
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
            var product = await productRepository.GetProductWithCategory(id)
                ?? throw new KeyNotFoundException($"Product with id {id} not found.");

            product.Name = productUpdateDTO.Name ?? product.Name;
            product.Description = productUpdateDTO.Description ?? product.Description;
            product.ImageUrl = productUpdateDTO.ImageUrl ?? product.ImageUrl;
            product.Price = productUpdateDTO.Price ?? product.Price;

            if (productUpdateDTO.CategoryIds != null)
            {
                var currentCategoryIds = product.Categories.Select(c => c.Id).ToList();

                var toAddCategories = productUpdateDTO.CategoryIds.Except(currentCategoryIds).ToList();
                var toRemoveCategories = currentCategoryIds.Except(productUpdateDTO.CategoryIds).ToList();

                foreach (var categoryId in toAddCategories)
                {
                    var category = await categoryRepository.GetByIdAsync(categoryId);
                    if (category == null)
                    {
                        throw new KeyNotFoundException($"Category with id {categoryId} not found.");
                    }
                    product.Categories.Add(category);
                }

                foreach (var categoryId in toRemoveCategories)
                {
                    var categoryToRemove = product.Categories.FirstOrDefault(c => c.Id == categoryId);
                    if (categoryToRemove != null)
                    {
                        product.Categories.Remove(categoryToRemove);
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
