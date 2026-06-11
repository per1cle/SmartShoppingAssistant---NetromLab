using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SmartShoppingAssistant.BussinesLogic.Models;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.BussinesLogic.Tools;
using System.ComponentModel;

namespace SmartShoppingAssistant.BussinesLogic.Agents;

public class PromotionCheckerAgent(IChatClient chatClient, IPromotionService promotionService) : IPromotionCheckerAgent
{
    public ChatClientAgent Build(string cartJson)
    {
        return new ChatClientAgent(
            chatClient,
            new ChatClientAgentOptions
            {
                Name = "PromotionChecker",
                Description = "Checks promotions for cart items",
                ChatOptions = new ChatOptions
                {
                    Instructions = $"""
                        You check promotions. Here is the current cart:
                        {cartJson}

                        1. Call GetPromotionsForProduct for each product in the cart.
                        2. Compare each promotion's rules against the cart quantities/totals.
                        3. For near-miss deals, calculate the savings the user would get.
                        4. For "Buy X Get Y" deals, the user needs to add X+Y items in the cart to get the promotion, so calculate how many more items they need to add to get the deal.
                        """,
                    ResponseFormat = ChatResponseFormat.ForJsonSchema<PromotionAnalysis>(),
                    Tools =
                    [
                        AIFunctionFactory.Create(
                            ([Description("The product ID to check")] int productId) =>
                                ShoppingTools.GetPromotionForProduct(productId, promotionService),
                            "GetPromotionsForProduct",
                            "Get all active promotions that apply to a specific product (by product ID or its category)."
                        )
                    ]
                }
            },
            null!,
            null!
        );
    }
}