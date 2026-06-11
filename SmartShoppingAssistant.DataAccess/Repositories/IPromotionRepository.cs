using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public interface IPromotionRepository : IRepository<Promotion>
{
    Task<List<Promotion>> GetForProductAsync(int productId);
    Task<List<Promotion>> GetActivePromotionsAsync();
}