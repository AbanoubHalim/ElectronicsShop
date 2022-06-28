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
        Task<Result<Discount>> GetDiscount(Guid productId);
        Task<Result<Discount>> PutDiscount(Guid productId, Discount discount);
        Task<Result<Discount>> PostDiscount(Discount discount);
        Task<Result<Discount>> DeleteProductDiscount(Guid id);
    }
    public class DiscountService:IDiscountService
    {
        private readonly ShopContext _context;
        public DiscountService(ShopContext context)
        {
            _context = context;
        }

        public async Task<Result<Discount>> DeleteProductDiscount(Guid productId)
        {
            var discount = await _context.Discount.SingleOrDefaultAsync(a => a.ProductId == productId);
            if (discount == null)
            {
                return new Result<Discount>
                {
                    Success = false,
                    ResponseMessage = "There is no discount with this id"
                };
            }
            _context.Discount.Remove(discount);
            await _context.SaveChangesAsync();

            return new Result<Discount>
            {
                Success = true,
                ResponseMessage = "Discount Deleted Successfully"
            };
        }

        public async Task<Result<Discount>> GetDiscount(Guid productId)
        {
                var discount = await _context.Discount.SingleOrDefaultAsync(o => o.ProductId == productId);
                if (discount == null)
                {
                    return new Result<Discount>
                    {
                        Success = false,
                        ResponseMessage = "There is no discount with this id"
                    };
                }
                return new Result<Discount>
                {
                    Success = true,
                    ResponseObject = discount
                };
        }

        public async Task<Result<Discount>> PostDiscount(Discount discount)
        {
            _context.Discount.Add(discount);
            await _context.SaveChangesAsync();

            return new Result<Discount>
            {
                Success = true,
                ResponseObject = discount,
                ResponseMessage = "Discount Added Successfully"
            };
        }

        public async Task<Result<Discount>> PutDiscount(Guid productId, Discount discount)
        {
            var discountData = await _context.Discount.SingleOrDefaultAsync(d => d.ProductId == productId);
            if (discountData == null)
            {
                return new Result<Discount>
                {
                    Success = false,
                    ResponseMessage = "There is no Discount with this id"
                };
            }
            discountData.ProductId = discount.ProductId;
            discountData.OnePieceDiscountPercentage = discount.OnePieceDiscountPercentage;
            discountData.MoreOnePieceDiscountPercentage = discount.MoreOnePieceDiscountPercentage;
            await _context.SaveChangesAsync();
            return new Result<Discount>
            {
                Success = true,
                ResponseObject = discountData,
                ResponseMessage="Discount information updated successfully",
            };
        }
    }
}
