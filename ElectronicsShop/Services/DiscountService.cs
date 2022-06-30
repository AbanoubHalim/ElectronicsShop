using ElectronicsShop.DTOs;
using ElectronicsShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Services
{
    public interface IDiscountService
    {
        Task<Discount> GetDiscount(Guid productId);
        Task PutDiscount(Guid productId, Discount discount);
        Task PostDiscount(Discount discount);
        Task DeleteProductDiscount(Discount discount);
    }
    public class DiscountService:IDiscountService
    {
        private readonly ShopContext _context;
        public DiscountService(ShopContext context)
        {
            _context = context;
        }

        public async Task DeleteProductDiscount(Discount discount)
        {
            _context.Discount.Remove(discount);
            await _context.SaveChangesAsync();
        }

        public async Task<Discount> GetDiscount(Guid productId)
        {
            return await _context.Discount.SingleOrDefaultAsync(o => o.ProductId == productId);  
        }

        public async Task PostDiscount(Discount discount)
        {
            discount.Id = Guid.NewGuid();
            _context.Discount.Add(discount);
            await _context.SaveChangesAsync();
        }

        public async Task PutDiscount(Guid productId, Discount discount)
        {
            var discountData = await _context.Discount.SingleOrDefaultAsync(d => d.ProductId == productId);
            discountData.ProductId = discount.ProductId;
            discountData.OnePieceDiscountPercentage = discount.OnePieceDiscountPercentage;
            discountData.MoreOnePieceDiscountPercentage = discount.MoreOnePieceDiscountPercentage;
            await _context.SaveChangesAsync();
            
        }
    }
}
