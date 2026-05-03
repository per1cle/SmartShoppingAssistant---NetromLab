using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories
{
    public interface IProductRepository: IRepository<Product>
    {
        Task<Product?> GetProductWithCategory(int id);
        Task<List<Product>> GetAllProductsWithCategory(int? categoryId = null);
    }
}
