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

namespace ElectronicsShop.Controllers
{
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
        public async Task<Result<Discount>> GetDiscount(Guid productId)
        {
            return await discountService.GetDiscount(productId);
        }

        [HttpPut("{productId}")]
        public async Task<Result<Discount>> PutDiscount(Guid productId, Discount discount)
        {
            if(!ModelState.IsValid)
            {
                return new Result<Discount>
                {
                    ResponseMessage = "Enter correct data and try again",
                    Success = false,
                };
            }
            return await discountService.PutDiscount(productId, discount);
        }

        [HttpPost]
        public async Task<Result<Discount>> PostDiscount(Discount discount)
        {
            if(!ModelState.IsValid)
            {
                return new Result<Discount>
                {
                    ResponseMessage = "Enter correct data and try again",
                    Success = false,
                };
            }

            return await discountService.PostDiscount(discount);
        }

        // DELETE: api/Discount/5
        [HttpDelete("{productId}")]
        public async Task<Result<Discount>> DeleteProductDiscount(Guid productId)
        {
            return await discountService.DeleteProductDiscount(productId);
        }

      
    }
}
