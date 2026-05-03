using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BussinesLogic.DTOs.CartItemsDTOs;
using SmartShoppingAssistant.BussinesLogic.Services;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;
using SmartShoppingAssistant.DataAccess.Entities;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartItemsController(ICartItemsService cartService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<CartSummaryDTO>> GetAll()
        {
            try
            {
                var cart = await cartService.GetCurrentCart();
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        [HttpPost("items")]
        public async Task<ActionResult<CartItemsGetDTO>> Add(CartItemsCreateDTO cartItemsCreateDTO)
        {
            try
            {
                var cartItem = await cartService.AddCartItemAsync(cartItemsCreateDTO);
                return Ok(cartItem);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("items/{id}")]
        public async Task<ActionResult<CartItemsGetDTO>> Update(int id, CartItemsUpdateDTO cartItemsUpdateDTO)
        {
            try
            {
                var updatedCart = await cartService.UpdateCartItemAsync(id, cartItemsUpdateDTO);
                return Ok(updatedCart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("items/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await cartService.DeleteCartItemAsync(id);
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

        [HttpDelete]
        public async Task<ActionResult> ClearCart()
        {
            try
            {
                await cartService.ClearCartAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
