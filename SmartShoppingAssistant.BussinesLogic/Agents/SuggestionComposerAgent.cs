using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.BussinesLogic.Tools;
using System.ComponentModel;

namespace SmartShoppingAssistant.BussinesLogic.Agents
{
    public class SuggestionComposerAgent(IChatClient chatClient, IProductService productService): ISuggestionComposer
    {
        public ChatClientAgent Build(string cartJson, string categoriesJson) // ,string promotionAnalysisJson)
        {
            return new ChatClientAgent(
                chatClient,
                new ChatClientAgentOptions
                {
                    Name = "SuggestionComposer",
                    Description = "Composes product suggestions based on cart and category data",
                    ChatOptions = new ChatOptions
                    {
                        Instructions = $@"
                        You create shopping suggestions based on the promotion analysis from the
                        previous agent and the current cart (which includes category info):
                        {cartJson}

                        Available categories in our store:
                        {categoriesJson}

                        Rules:
                        1. The previous agent's output contains active deals (already qualifying) and
                           near-miss deals (almost qualifying). Use both to generate suggestions.
                        2. For category-level promotions, suggest items from the SAME category that
                           would help trigger the deal (e.g. adding another Electronics item to meet
                           a category quantity threshold).
                        3. Use GetProductsByCategory to find real products — only
                           suggest products that the tools actually returned.
                        4. Include calculated savings for each suggestion where applicable.
                        5. Also suggest complementary products based on what's in the cart
                           (e.g. phone case for a phone, charger for a laptop).
                        6. Max 5 suggestions, prioritizing those with the highest savings.",
                        ResponseFormat = ChatResponseFormat.ForJsonSchema<Models.AnalysisResponse>(),
                        Tools =
                        [   //search products
                            AIFunctionFactory.Create(
                            ([Description("The category ID to check")] int categoryId) =>
                                ShoppingTools.GetProductsByCategory(categoryId, productService),
                            "GetProductsByCategory",
                            "Get available products by category ID to suggest to the user."
                        )
                        ]
                    }
                },
                null!,
                null!
            );
        }
    }
}
