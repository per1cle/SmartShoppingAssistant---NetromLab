using SmartShoppingAssistant.BussinesLogic.DTOs;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Repositories;

namespace SmartShoppingAssistant.BussinesLogic.Services
{
    public class ProductService(IRepository<Product> productRepository) : IProductService
    {
        public async Task<ProductGetDTO> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            return new ProductGetDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price
            };
        }

        public async Task DeleteAsync(int id)
        {
            await productRepository.DeleteAsync(id);
        }

        public async Task<List<ProductGetDTO>> GetAllAsync()
        {
            var products = await productRepository.GetAllAsync();
            return products.Select(product => new ProductGetDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price
            }).ToList();
        }

        public async Task<ProductGetDTO> AddAsync(ProductCreateDTO productCreateDTO)
        {
            var product = new Product
            {
                Name = productCreateDTO.Name,
                Description = productCreateDTO.Description,
                ImageUrl = productCreateDTO.ImageUrl,
                Price = productCreateDTO.Price
            };
            var addedProduct = await productRepository.AddAsync(product);
            return new ProductGetDTO
            {
                Id = addedProduct.Id,
                Name = addedProduct.Name,
                Description = addedProduct.Description,
                ImageUrl = addedProduct.ImageUrl,
                Price = addedProduct.Price
            };
        }

        public async Task<ProductGetDTO> UpdateAsync(ProductUpdateDTO productUpdateDTO)
        {
            var existingProduct = await productRepository.GetByIdAsync(productUpdateDTO.Id);

            existingProduct.Name = productUpdateDTO.Name;
            existingProduct.Description = productUpdateDTO.Description;
            existingProduct.ImageUrl = productUpdateDTO.ImageUrl;
            existingProduct.Price = productUpdateDTO.Price;

            var updatedProduct = await productRepository.UpdateAsync(existingProduct);

            return new ProductGetDTO
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                ImageUrl = updatedProduct.ImageUrl,
                Price = updatedProduct.Price
            };
        }
    }
}
