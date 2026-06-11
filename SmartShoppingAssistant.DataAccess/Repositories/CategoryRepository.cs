using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class CategoryRepository(SmartShoppingAssistantDbContext context)
    : BaseRepository<Category>(context), ICategoryRepository
{
    public async Task<List<Category>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await GetAllAsQueryable()
            .Where(c => ids.Contains(c.Id))
            .ToListAsync();
    }
}