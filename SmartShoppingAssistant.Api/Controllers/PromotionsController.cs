using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BussinesLogic.DTOs.PromotionDTOs;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController(IPromotionService promotionService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionGetDTO>> GetById(int id)
        {
            try
            {
                var promotion = await promotionService.GetPromotionByIdAsync(id);
                return Ok(promotion); ;
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<PromotionGetDTO>>> GetAll()
        {
            try
            {
                var promotions = await promotionService.GetAllPromotionsAsync();
                return Ok(promotions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        [HttpPost]
        public async Task<ActionResult<PromotionGetDTO>> Add(PromotionCreateDTO promotionCreateDTO)
        {
            try
            {
                var promotion = await promotionService.AddPromotionAsync(promotionCreateDTO);
                return CreatedAtAction(nameof(GetById), new { id = promotion.Id }, promotion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PromotionGetDTO>> Update(int id, PromotionUpdateDTO promotionUpdateDTO)
        {
            try
            {
                var updatedPromotion = await promotionService.UpdatePromotionAsync(id, promotionUpdateDTO);
                return Ok(updatedPromotion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await promotionService.DeletePromotionAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

}
