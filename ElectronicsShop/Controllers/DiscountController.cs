using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronicsShop.Models;
using ElectronicsShop.Services;
using ElectronicsShop.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ElectronicsShop.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService discountService;
        public DiscountController(IDiscountService discountService)
        {
            this.discountService = discountService;
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Discount>> GetDiscount(Guid productId)
        {
            return await discountService.GetDiscount(productId);
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<Discount>> PutDiscount(Guid productId, Discount discount)
        {
            var discountData = await discountService.GetDiscount(productId);
            if (discountData == null)
            {
                return NotFound("Sorry this discount is not exist");
            }
            else if (productId != discount.ProductId)
            {
                return BadRequest();
            }
            await discountService.PutDiscount(productId, discount);
            return NoContent();           
        }

        [HttpPost]
        public async Task<ActionResult<Discount>> PostDiscount(Discount discount)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            else
            {
                await discountService.PostDiscount(discount);
                return Ok(discount);
            }
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<Discount>> DeleteProductDiscount(Guid productId)
        {
            var discount = await discountService.GetDiscount(productId);
            if (discount == null)
            {
                return NotFound("Sorry this order is not exist");
            }
            await discountService.DeleteProductDiscount(discount);
            return NoContent();
        }

      
    }
}
