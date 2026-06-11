using System.ComponentModel;
using System.Text.Json.Serialization;

namespace SmartShoppingAssistant.BussinesLogic.Models
{
    [Description("Cart analysis result with product suggestions")]
    public sealed class AnalysisResponse
    {
        [JsonPropertyName("summary")]
        public string Summary { get; set; } = "";

        [JsonPropertyName("suggestions")]
        public List<Suggestion> Suggestions { get; set; } = [];
    }

    public sealed class Suggestion
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = "";

        [JsonPropertyName("productPrice")]
        public decimal ProductPrice { get; set; }

        [JsonPropertyName("productQuantity")]
        public int ProductQuantity { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = "";

        [JsonPropertyName("savings")]
        public decimal? Savings { get; set; }
    }

}
