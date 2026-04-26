using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BussinesLogic.DTOs;
using SmartShoppingAssistant.BussinesLogic.Services.Interfaces;

namespace SmartShoppingAssistant.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductGetDTO>> GetById(int id)
        {
            try
            {
                var product = await productService.GetByIdAsync(id);
                return Ok(product); ;
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await productService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductGetDTO>>> GetAll()
        {
            try
            {
                var products = await productService.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);    //bad request?
            }

        }

        [HttpPost]
        public async Task<ActionResult<ProductGetDTO>> Add(ProductCreateDTO productCreateDTO)
        {
            try
            {
                var product = await productService.AddAsync(productCreateDTO);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductGetDTO>> Update(int id, ProductUpdateDTO productUpdateDTO)
        {
            try
            {
                productUpdateDTO.Id = id;
                var updatedProduct = await productService.UpdateAsync(productUpdateDTO);
                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
