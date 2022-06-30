using ElectronicsShop.DTOs;
using ElectronicsShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Services
{ 
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategory();
        Task<Category> GetCategory(Guid id);
        Task PutCategory(Guid id, Category category);
        Task PostCategory(Category category);
        Task DeleteCategory(Category category);
    }

    public class CategoryService: ICategoryService
    {
        private readonly ShopContext _context;
        public CategoryService(ShopContext context)
        {
            _context = context;
        }

        public async Task DeleteCategory(Category category)
        {
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetCategory()
        {
           return await _context.Category.ToListAsync();
        }

        public async Task<Category> GetCategory(Guid id)
        {
            return await _context.Category.SingleOrDefaultAsync(c=>c.CategoryId==id);
        }

        public async Task PostCategory(Category category)
        {
            category.CategoryId = Guid.NewGuid();
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task PutCategory(Guid id, Category category)
        {
                var categoryData = _context.Category.SingleOrDefault(c => c.CategoryId == id);
                categoryData.Name = category.Name;
                await _context.SaveChangesAsync();
        }
    }
}
