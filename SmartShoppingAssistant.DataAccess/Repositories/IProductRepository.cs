using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories
{
    public interface IProductRepository: IRepository<Product>
    {
        Task<List<Product>> GetAllAsync(int? categoryId);
        Task<Product?> GetByIdWithCategoriesAsync(int id);
        Task<List<Product>> SearchAsync(string query);
        Task<List<Product>> GetByCategoryAsync(int categoryId);
    }
}
