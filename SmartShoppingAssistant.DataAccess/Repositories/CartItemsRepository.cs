using Microsoft.EntityFrameworkCore;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.DataAccess.Repositories;

public class CartItemsRepository(SmartShoppingAssistantDbContext context)
    : BaseRepository<CartItems>(context), ICartItemsRepository
{
    private IQueryable<CartItems> WithProduct() =>
        GetAllAsQueryable().Include(ci => ci.Product);

    public async Task<List<CartItems>> GetAllWithProductAsync()
    {
        return await WithProduct().ToListAsync();
    }

    public async Task<CartItems?> GetByProductIdAsync(int productId)
    {
        return await WithProduct().FirstOrDefaultAsync(ci => ci.ProductId == productId);
    }

    public async Task<CartItems?> GetByIdWithProductAsync(int id)
    {
        return await WithProduct().FirstOrDefaultAsync(ci => ci.Id == id);
    }

    public async Task ClearAsync()
    {
        context.CartItems.RemoveRange(context.CartItems);
        await context.SaveChangesAsync();
    }

    public async Task<List<CartItems>> GetProductAndCategoriesAsync()
    {
        return await context.CartItems
            .Include(ci => ci.Product)            
            .ThenInclude(p => p.Categories)   
            .ToListAsync();
    }
}