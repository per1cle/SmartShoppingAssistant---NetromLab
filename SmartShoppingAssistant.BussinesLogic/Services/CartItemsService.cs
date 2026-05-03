using SmartShoppingAssistant.BussinesLogic.DTOs.CartItemsDTOs;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;
using SmartShoppingAssistant.DataAccess.Entities.Enums;
using SmartShoppingAssistant.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShoppingAssistant.BussinesLogic.Services
{
    public class CartItemsService(IRepository<CartItems> cartItemsRepository,
        IRepository<Product> productRepository,
        IRepository<Promotion> promotionRepopsitory,
        IProductRepository productRepositoryWithCategory) : ICartItemsService
    {
        public async Task<CartSummaryDTO> GetCurrentCart()  //with totals
        {
            var cartItems = await cartItemsRepository.GetAllAsync();
            var allPromotion = await promotionRepopsitory.GetAllAsync();
            var activePromotions = allPromotion.Where(p => p.IsActive).ToList();

            var summary = new CartSummaryDTO();
            decimal totalDiscount = 0;

            foreach (var item in cartItems)
            {
                var product = await productRepositoryWithCategory.GetProductWithCategory(item.ProductId);
                if (product == null) continue;

                var lineTotal = product.Price * item.Quantity;
                summary.Items.Add(new CartItemSummaryDTO
                {
                    Id = item.Id,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    LineTotal = lineTotal
                });

                summary.Subtotal += lineTotal;

                // Apply Products/Category Promotions
                var productCategoryIds = product.Categories?.Select(c => c.Id).ToList() ?? new List<int>();

                var promo = activePromotions.FirstOrDefault(p =>
                    p.ProductId == product.Id ||
                    (p.CategoryId != null && productCategoryIds.Contains(p.CategoryId.Value)));

                if (promo != null)
                {
                    if (promo.Type == PromotionType.Quantity && item.Quantity >= promo.Threshold)
                    {
                        if (promo.Reward == PromotionReward.FreeItems)
                        {
                            int timesQualified = (int)(item.Quantity / promo.Threshold);
                            int totalFreeItems = timesQualified * promo.RewardValue;

                            totalDiscount += totalFreeItems * product.Price;
                        }
                        else if (promo.Reward == PromotionReward.PercentDiscount)
                        {
                            totalDiscount += lineTotal * (promo.RewardValue / 100m);
                        }
                    }
                }
            }
            // Apply Cart Promotions
            var cartPromos = activePromotions.Where(p => p.Type == PromotionType.CartTotal && p.ProductId == null && p.CategoryId == null).ToList();

            decimal currentTotal = summary.Subtotal - totalDiscount;

            foreach (var promo in cartPromos)
            {
                if (currentTotal >= promo.Threshold)
                {
                    if (promo.Reward == PromotionReward.PercentDiscount)
                    {
                        decimal discountPercentage = promo.RewardValue / 100m;
                        totalDiscount += currentTotal * discountPercentage;
                        currentTotal -= currentTotal * discountPercentage;
                    }
                }
            }

            summary.Discount = totalDiscount;
            summary.FinalTotal = summary.Subtotal - totalDiscount;

            if (summary.FinalTotal < 0) summary.FinalTotal = 0;

            return summary;
        }

        public async Task<CartItemsGetDTO> AddCartItemAsync(CartItemsCreateDTO cartItemsCreateDTO)
        {
            var product = await productRepository.GetByIdAsync(cartItemsCreateDTO.ProductId)
                ?? throw new KeyNotFoundException($"Product with id {cartItemsCreateDTO.ProductId} not found.");

            if (cartItemsCreateDTO.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            var cartItems = await cartItemsRepository.GetAllAsync();
            var existingItem = cartItems.FirstOrDefault(ci => ci.ProductId == cartItemsCreateDTO.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItemsCreateDTO.Quantity;
                var updatedItem = await cartItemsRepository.UpdateAsync(existingItem);
                return MapToCartItemsGetDTO(updatedItem);

            }

            var newCartItem = new CartItems
            {
                ProductId = cartItemsCreateDTO.ProductId,
                Quantity = cartItemsCreateDTO.Quantity
            };

            var addedCartItem = await cartItemsRepository.AddAsync(newCartItem);
            return MapToCartItemsGetDTO(addedCartItem);
        }

        public async Task<CartItemsGetDTO> UpdateCartItemAsync(int id, CartItemsUpdateDTO cartItemsUpdateDTO)
        {
            var cartItem = await cartItemsRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Cart item with id {id} not found.");

            if (cartItemsUpdateDTO.Quantity.HasValue)
            {
                if (cartItemsUpdateDTO.Quantity.Value <= 0)
                {
                    throw new ArgumentException("Quantity must be greater than zero.");
                }
                cartItem.Quantity = cartItemsUpdateDTO.Quantity.Value;
            }

            var updatedCartItem = await cartItemsRepository.UpdateAsync(cartItem);
            return MapToCartItemsGetDTO(updatedCartItem);
        }

        public async Task DeleteCartItemAsync(int id)
        {
            var cartItem = await cartItemsRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Cart item with id {id} not found.");
            await cartItemsRepository.DeleteAsync(cartItem);
        }

        public async Task ClearCartAsync()
        {
            var cartItems = await cartItemsRepository.GetAllAsync();

            foreach (var item in cartItems)
            {
                await cartItemsRepository.DeleteAsync(item);
            }
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
