using SmartShoppingAssistant.DataAccess.Entities.Enums;

namespace SmartShoppingAssistant.BussinesLogic.DTOs.PromotionDTOs
{
    public class AppliedPromotionDTO
    {
        public string PromotionName { get; set; } = null!;
        public decimal Discount { get; set; }
    }
}
