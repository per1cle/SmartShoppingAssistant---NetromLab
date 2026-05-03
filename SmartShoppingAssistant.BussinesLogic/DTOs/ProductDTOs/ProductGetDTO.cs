using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.BussinesLogic.DTOs.ProductDTOs
{
    public class ProductGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();

    }
}
