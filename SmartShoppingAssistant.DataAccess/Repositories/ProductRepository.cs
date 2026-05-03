using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories
{
    public class ProductRepository(SmartShoppingAssistantDbContext context) : BaseRepository<Product>(context), IProductRepository
    {
        private readonly SmartShoppingAssistantDbContext _context = context;
        public async Task<Product?> GetProductWithCategory(int id)
        {
            return await _context.Products.Include(p => p.Categories).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetAllProductsWithCategory(int? categoryId = null)
        {
            var query = _context.Products.Include(p => p.Categories).AsQueryable();

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.Categories.Any(c => c.Id == categoryId.Value));
            }
            return await query.ToListAsync();
        }
    }
}
