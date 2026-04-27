using SmartShoppingAssistant.BussinesLogic.DTOs;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BussinesLogic.Services
{
    public class ProductService(IRepository<Product> productRepository) : IProductService
    {
        public async Task<ProductGetDTO> GetProductByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Product with id {id} not found.");

            return MapToProductGetDTO(product);
        }

        public async Task<List<ProductGetDTO>> GetAllProductsAsync()
        {
            var products = await productRepository.GetAllAsync();
            
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

            var addedProduct = await productRepository.AddAsync(product);
            
            return MapToProductGetDTO(addedProduct);
        }

        public async Task<ProductGetDTO> UpdateProductAsync(int id, ProductUpdateDTO productUpdateDTO)
        {
            var existingProduct = await productRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Product with id {id} not found.");

            if (productUpdateDTO.Name != null)
                existingProduct.Name = productUpdateDTO.Name;

            if (productUpdateDTO.Description != null)
                existingProduct.Description = productUpdateDTO.Description;

            if (productUpdateDTO.ImageUrl != null)
                existingProduct.ImageUrl = productUpdateDTO.ImageUrl;

            if (productUpdateDTO.Price != null)
                existingProduct.Price = productUpdateDTO.Price.Value;

            var updatedProduct = await productRepository.UpdateAsync(existingProduct);

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
                Price = product.Price
            };
        }
    }
}
