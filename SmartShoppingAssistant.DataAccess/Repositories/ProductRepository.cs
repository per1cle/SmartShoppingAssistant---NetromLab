using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories
{
    public class ProductRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Product>(context), IProductRepository
    {
        private IQueryable<Product> WithCategories() =>
        GetAllAsQueryable().Include(p => p.Categories);
        public async Task<Product?> GetByIdWithCategoriesAsync(int id)
        {
            return await WithCategories().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetAllAsync(int? categoryId)
        {
            var query = WithCategories();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.Categories.Any(c => c.Id == categoryId.Value));
            }
            return await query.ToListAsync();
        }

        public async Task<List<Product>> SearchAsync(string query)
        {
            return await WithCategories()
                .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<Product>> GetByCategoryAsync(int categoryId)
        {
            return await WithCategories()
                .Where(p => p.Categories.Any(c => c.Id == categoryId))
                .Take(10)
                .ToListAsync();
        }
    }
}
