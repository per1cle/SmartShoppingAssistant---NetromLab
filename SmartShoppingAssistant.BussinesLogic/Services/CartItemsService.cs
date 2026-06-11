using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using SmartShoppingAssistant.BussinesLogic.Agents;
using SmartShoppingAssistant.BussinesLogic.DTOs.CartItemsDTOs;
using SmartShoppingAssistant.BussinesLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BussinesLogic.Models;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using SmartShoppingAssistant.DataAccess.Repositories;
using System.Text.Json;

namespace SmartShoppingAssistant.BussinesLogic.Services
{
    public class CartItemsService(ICartItemsRepository cartItemsRepository,
        IProductRepository productRepository,
        IPromotionRepository promotionRepository,
        ICategoryRepository categoryRepository,
        IPromotionCheckerAgent promotionCheckerAgent,
        ISuggestionComposer suggestionComposerAgent) : ICartItemsService
    {
        public async Task<CartSummaryDTO> GetCurrentCart()
        {
            var cartSummary = new CartSummaryDTO();

            var cartItems = await cartItemsRepository.GetProductAndCategoriesAsync();
            if (!cartItems.Any()) return cartSummary;

            //cantitatea totala pentru product si category     key e id si valoarea e cantitatea
            var totalQtyPerProduct = cartItems.GroupBy(i => i.ProductId).ToDictionary(g => g.Key, g => g.Sum(i => i.Quantity));

            var totalQtyPerCategory = cartItems
                .SelectMany(i => i.Product.Categories.Select(c => new { c.Id, i.Quantity }))
                .GroupBy(x => x.Id)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

            var allPromotions = await promotionRepository.GetActivePromotionsAsync();

            foreach (var item in cartItems)
            {
                var product = item.Product;
                decimal itemSubtotal = product.Price * item.Quantity;
                cartSummary.Subtotal += itemSubtotal;

                cartSummary.Items.Add(new CartItemsSummaryDTO
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    Subtotal = itemSubtotal
                });

                var catIds = product.Categories.Select(c => c.Id).ToList(); 

                var applicablePromos = allPromotions.Where(p =>
                    (p.ProductId == item.ProductId) ||
                    (p.CategoryId.HasValue && catIds.Contains(p.CategoryId.Value))
                ).ToList();

                foreach (var promo in applicablePromos.Where(p => p.Type == PromotionType.Quantity))
                {
                    bool thresholdMet = false;
                    if (promo.ProductId.HasValue)
                        thresholdMet = totalQtyPerProduct[promo.ProductId.Value] >= promo.Threshold;
                    else if (promo.CategoryId.HasValue)
                        thresholdMet = totalQtyPerCategory[promo.CategoryId.Value] >= promo.Threshold;

                    if (thresholdMet)
                    {
                        decimal itemDiscount = 0;
                        //pt free items
                        if (promo.Reward == PromotionReward.FreeItems)
                        {
                            //daca e 2+1 gratis => 2 in cos -> platesti 2 ; 3 in cos -> platesti 2
                            int bundleSize = (int)promo.Threshold + promo.RewardValue;  //pt 2+1 bundle=3 pt ca trebuie sa avem 3 produse in cos ca sa se aplice
                            int sets = item.Quantity / bundleSize;   //de cate ori se aplica
                            itemDiscount = sets * promo.RewardValue * product.Price;   // cat va trebui sa scadem din total
                        }
                        //pr percent discount
                        else if (promo.Reward == PromotionReward.PercentDiscount)
                        {
                            itemDiscount = itemSubtotal * ((decimal)promo.RewardValue / 100);
                        }

                        if (itemDiscount > 0)
                        {
                            cartSummary.TotalDiscount += itemDiscount;
                            cartSummary.AppliedPromotions.Add(new AppliedPromotionDTO   //promotia este aplicata
                            {
                                PromotionName = $"{promo.Name} (pentru {product.Name})",
                                Discount = itemDiscount
                            });
                        }
                    }
                }
            }

            // promotii pe tot cosul 
            var globalPromos = allPromotions.Where(p => p.Type == PromotionType.CartTotal);

            foreach (var promo in globalPromos)
            {
                if (cartSummary.Subtotal >= promo.Threshold)
                {
                    decimal globalDiscount = cartSummary.Subtotal * ((decimal)promo.RewardValue / 100);
                    cartSummary.TotalDiscount += globalDiscount;
                    cartSummary.AppliedPromotions.Add(new AppliedPromotionDTO
                    {
                        PromotionName = promo.Name,
                        Discount = globalDiscount
                    });
                }
            }

            cartSummary.Total = Math.Max(0, cartSummary.Subtotal - cartSummary.TotalDiscount);    //sa nu fie sub 0
            return cartSummary;
        }

        public async Task<CartSummaryDTO> AddCartItemAsync(CartItemsCreateDTO cartItemsCreateDTO)
        {
           if (cartItemsCreateDTO.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

           var product = await productRepository.GetByIdAsync(cartItemsCreateDTO.ProductId)
                ?? throw new KeyNotFoundException($"Product with id {cartItemsCreateDTO.ProductId} not found.");

            var existingItem =  await cartItemsRepository.GetByProductIdAsync(cartItemsCreateDTO.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItemsCreateDTO.Quantity;
                await cartItemsRepository.UpdateAsync(existingItem);

            }
            else
            {
                var newCartItem = new CartItems
                {
                    ProductId = cartItemsCreateDTO.ProductId,
                    Quantity = cartItemsCreateDTO.Quantity
                };
                await cartItemsRepository.AddAsync(newCartItem);
            }

            return await GetCurrentCart();
        }

        public async Task<CartSummaryDTO> UpdateCartItemAsync(int id, CartItemsUpdateDTO cartItemsUpdateDTO)
        {
            var cartItem = await cartItemsRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Cart item with id {id} not found.");

            if (cartItemsUpdateDTO.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }
            cartItem.Quantity = cartItemsUpdateDTO.Quantity;
            

            await cartItemsRepository.UpdateAsync(cartItem);
            return await GetCurrentCart();
        }

        public async Task DeleteCartItemAsync(int id)
        {
            var cartItem = await cartItemsRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Cart item with id {id} not found.");
            await cartItemsRepository.DeleteAsync(cartItem);
        }

        public async Task ClearCartAsync() => await cartItemsRepository.ClearAsync();

        public async Task<AnalysisResponse> AnalyzeCartAsync()
        {
            var cart = await cartItemsRepository.GetProductAndCategoriesAsync();
            var categories = await categoryRepository.GetAllAsync();

            var cartJson = JsonSerializer.Serialize(cart.Select(c => new
            {
                c.ProductId,
                c.Product.Price,
                c.Quantity,
                LineTotal = c.Product.Price * c.Quantity,
                CategoryIds = c.Product.Categories.Select(cat => new {CategoryId = cat.Id, CategoryName = cat.Name}).ToList()
            }));

            var categoriesJson = JsonSerializer.Serialize(categories.Select(c => new
            {
                CategoryId = c.Id,
                CategoryName = c.Name
            }));

            var promotionAgent = promotionCheckerAgent.Build(cartJson);
            var suggestionAgent = suggestionComposerAgent.Build(cartJson, categoriesJson);

            var workflow = new WorkflowBuilder(promotionAgent).AddEdge(promotionAgent, suggestionAgent)
                .WithOutputFrom(suggestionAgent)
                .Build();

            var chatMessage = new List<ChatMessage>
            {
                new(ChatRole.User, "Analyze the cart and suggest improvements.")
            };

            await using var result = await InProcessExecution.RunStreamingAsync(workflow, chatMessage);
            await result.TrySendMessageAsync(new TurnToken(emitEvents: true));

            var jsonBuilder = new System.Text.StringBuilder();

            await foreach(var message in result.WatchStreamAsync())
            {
                if(message is AgentResponseUpdateEvent update && update.ExecutorId.StartsWith("SuggestionComposer"))
                {
                    jsonBuilder.Append(update.Update.Text);
                }
                else if(message is WorkflowErrorEvent errorEvent)
                {
                    throw new InvalidOperationException(errorEvent.Exception?.Message);
                }
            }

            var json = jsonBuilder.ToString();
            return JsonSerializer.Deserialize<AnalysisResponse>(json) ?? throw new InvalidOperationException("Failed to deserialize analysis response.");
        }
        public static CartItemsGetDTO MapToCartItemsGetDTO(CartItems cartItem)
        {
            return new CartItemsGetDTO
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };
        }
    }
}
