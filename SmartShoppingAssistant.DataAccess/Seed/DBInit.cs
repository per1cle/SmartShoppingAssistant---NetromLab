using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace SmartShoppingAssistant.DataAccess
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(SmartShoppingAssistantDbContext context)
        {
            await context.Database.MigrateAsync();


            if (!await context.Categories.AnyAsync())
            {
                var categorii = new List<Category>
        {
            new Category { Name = "Alimente", Description = "Toate produsele de consum alimentar" },
            new Category { Name = "Lactate", Description = "Lapte, iaurt, unt si branzeturi" },
            new Category { Name = "Bauturi", Description = "Sucuri, apa si bauturi racoritoare" },
            new Category { Name = "Panificatie", Description = "Paine proaspata si produse de patiserie" },
            new Category { Name = "Legume", Description = "Legume proaspete de sezon" },
            new Category { Name = "Electronice", Description = "Dispozitive si accesorii IT" }
        };

                await context.Categories.AddRangeAsync(categorii);
                await context.SaveChangesAsync();
            }


            if (!await context.Products.AnyAsync())
            {
                var catAlimentare = await context.Categories.FirstAsync(c => c.Name == "Alimente");
                var catLactate = await context.Categories.FirstAsync(c => c.Name == "Lactate");
                var catBauturi = await context.Categories.FirstAsync(c => c.Name == "Bauturi");
                var catPanificatie = await context.Categories.FirstAsync(c => c.Name == "Panificatie");
                var catLegume = await context.Categories.FirstAsync(c => c.Name == "Legume");
                var catElectronice = await context.Categories.FirstAsync(c => c.Name == "Electronice");

                var produse = new List<Product>
        {
            new Product { Name = "Lapte 1.5% 1L", Description = "Lapte UHT degresat", Price = 6.50m, ImageUrl = "https://images.unsplash.com/photo-1550630997-aea8d3d982ed?q=80&w=764&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Categories = new List<Category> { catLactate, catAlimentare } },
                    new Product { Name = "Iaurt Grecesc", Description = "Iaurt cremos 10% grasime", Price = 4.20m, ImageUrl = "https://images.unsplash.com/photo-1728389617819-cddee5310e35?q=80&w=735&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Categories = new List<Category> { catLactate, catAlimentare } },
                    new Product { Name = "Unt 82%", Description = "Unt de masa premium 200g", Price = 16.50m, ImageUrl = "https://plus.unsplash.com/premium_photo-1700440539073-c769891a9e3f?q=80&w=688&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Categories = new List<Category> { catLactate, catAlimentare } },
                    
                    // OUA
                    new Product { Name = "Oua de tara 10 buc", Description = "Oua proaspete categoria A", Price = 14.00m, ImageUrl = "https://images.unsplash.com/photo-1587486913049-53fc88980cfc?w=500&q=80", Categories = new List<Category> { catAlimentare } },

                    // BAUTURI
                    new Product { Name = "Apa Plata 2L", Description = "Apa minerala necarbogazoasa", Price = 3.20m, ImageUrl = "https://images.unsplash.com/photo-1523362628745-0c100150b504?w=500&q=80", Categories = new List<Category> { catBauturi, catAlimentare } },
                    new Product { Name = "Sprite 2L", Description = "Bautura racoritoare cu lamaie", Price = 10.50m, ImageUrl = "https://images.unsplash.com/photo-1680404005217-a441afdefe83?q=80&w=764&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Categories = new List<Category> { catBauturi, catAlimentare } },
                    new Product { Name = "Pepsi 2L", Description = "Bautura racoritoare carbogazoasa", Price = 10.00m, ImageUrl = "https://images.unsplash.com/photo-1629203851122-3726ecdf080e?q=80&w=1229&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Categories = new List<Category> { catBauturi, catAlimentare } },

                    // PANIFICATIE 
                    new Product { Name = "Paine feliata 500g", Description = "Paine alba de grau", Price = 5.50m, ImageUrl = "https://images.unsplash.com/photo-1509440159596-0249088772ff?w=500&q=80", Categories = new List<Category> { catPanificatie, catAlimentare } },
                    new Product { Name = "Strudel cu visine", Description = "Produs patiserie cu umplutura fructe", Price = 3.80m, ImageUrl = "https://images.unsplash.com/photo-1601000938259-9e92002320b2?w=500&q=80", Categories = new List<Category> { catPanificatie, catAlimentare } },

                    // LEGUME 
                    new Product { Name = "Rosii cherry 250g", Description = "Rosii dulci calitatea I", Price = 8.00m, ImageUrl = "https://images.unsplash.com/photo-1592924357228-91a4daadcfea?w=500&q=80", Categories = new List<Category> { catLegume, catAlimentare } },
                    new Product { Name = "Castraveti Fabio", Description = "Castraveti proaspeti", Price = 4.50m, ImageUrl = "https://images.unsplash.com/photo-1589621316382-008455b857cd?q=80&w=1170&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D", Categories = new List<Category> { catLegume, catAlimentare } },
                    new Product { Name = "Ardei Kapia", Description = "Ardei rosu proaspat kg", Price = 12.00m, ImageUrl = "https://images.unsplash.com/photo-1563565375-f3fdfdbefa83?w=500&q=80", Categories = new List<Category> { catLegume, catAlimentare } },

                    // ELECTRONICE
                    new Product { Name = "Televizor LED 4K", Description = "Smart TV Diagonala 108cm", Price = 1450.00m, ImageUrl = "https://images.unsplash.com/photo-1593359677879-a4bb92f829d1?w=500&q=80", Categories = new List<Category> { catElectronice } },
                    new Product { Name = "Tastatura Mecanica", Description = "Tastatura RGB Gaming", Price = 220.00m, ImageUrl = "https://images.unsplash.com/photo-1595225476474-87563907a212?w=500&q=80", Categories = new List<Category> { catElectronice } },
                    new Product { Name = "Mouse Wireless", Description = "Mouse ergonomic optic", Price = 85.00m, ImageUrl = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=500&q=80", Categories = new List<Category> { catElectronice } }
        };

                await context.Products.AddRangeAsync(produse);
                await context.SaveChangesAsync();
            }


            if (!await context.Promotions.AnyAsync())
            {
                var pepsi = await context.Products.FirstAsync(p => p.Name == "Pepsi 2L");
                var strudel = await context.Products.FirstAsync(p => p.Name == "Strudel cu visine");
                var catLegume = await context.Categories.FirstAsync(c => c.Name == "Legume");
                var catElectronice = await context.Categories.FirstAsync(c => c.Name == "Electronice");
                var rosii = await context.Products.FirstAsync(p => p.Name == "Rosii cherry 250g");

                var promotii = new List<Promotion>
                {
                    new Promotion { Name = "Party Pack: 2+1 Pepsi", Type = PromotionType.Quantity, Threshold = 2, Reward = PromotionReward.FreeItems, RewardValue = 1, ProductId = pepsi.Id, IsActive = true },
            
                    new Promotion { Name = "Gustare dulce: 1+1 Strudel", Type = PromotionType.Quantity, Threshold = 1, Reward = PromotionReward.FreeItems, RewardValue = 1, ProductId = strudel.Id, IsActive = true },

                    new Promotion { Name = "Saptamana Sanatatii: 20% la Legume", Type = PromotionType.Quantity, Threshold = 3, Reward = PromotionReward.PercentDiscount, RewardValue = 20, CategoryId = catLegume.Id, IsActive = true },

                    new Promotion { Name = "Upgrade IT: 10% la Electronice", Type = PromotionType.Quantity, Threshold = 1, Reward = PromotionReward.PercentDiscount, RewardValue = 10, CategoryId = catElectronice.Id, IsActive = true },

                    new Promotion { Name = "Mega Shopping: 12 RON reducere la peste 400 RON", Type = PromotionType.CartTotal, Threshold = 400, Reward = PromotionReward.PercentDiscount, RewardValue = 12, IsActive = true },

                    new Promotion
                    {
                        Name = "Oferta Fermierului: 1+1 Gratis la Rosii",
                        Type = PromotionType.Quantity,
                        Threshold = 1,
                        Reward = PromotionReward.FreeItems,
                        RewardValue = 1,
                        ProductId = rosii.Id,
                        IsActive = true
                    },
                };

                await context.Promotions.AddRangeAsync(promotii);
                await context.SaveChangesAsync();
            }
        }
    }
}