using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface ICartItemsRepository : IRepository<CartItems>
{
    Task<List<CartItems>> GetAllWithProductAsync();
    Task<CartItems?> GetByProductIdAsync(int productId);
    Task<CartItems?> GetByIdWithProductAsync(int id);
    Task ClearAsync();
    Task<List<CartItems>> GetProductAndCategoriesAsync();
}
