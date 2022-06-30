using ElectronicsShop.DTOs;
using ElectronicsShop.Models;
using ElectronicsShop.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProduct();
        Task<IEnumerable<Product>> ProductsPerCategory(Guid id);
        Task<Product> GetProduct(Guid id);
        Task PutProduct(Guid id, Product product);
        Task PostProduct(Product product);
        Task DeleteProduct(Product product);
    }
    public class ProductService:IProductService
    {
        private readonly ShopContext _context;
        public ProductService(ShopContext context)
        {
            _context = context;
        }

        public async Task DeleteProduct(Product product)
        {
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProduct()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product> GetProduct(Guid id)
        {
            return await _context.Product.SingleOrDefaultAsync(p=>p.Id==id);
        }

        public async Task PostProduct(Product product)
        {
            product.Id = Guid.NewGuid();
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> ProductsPerCategory(Guid id)
        {
           return await _context.Product.Where(p => p.CategoryId == id).ToListAsync();
        }

        public async Task PutProduct(Guid id, Product product)
        {
            var productData = await _context.Product.SingleOrDefaultAsync(p => p.Id == id);
            productData.Name = product.Name;
            productData.PieceCount = product.PieceCount;
            productData.Price = product.Price;
            productData.Description = product.Description;
            productData.CategoryId = product.CategoryId;
            await _context.SaveChangesAsync();
            
        }
    }
}
