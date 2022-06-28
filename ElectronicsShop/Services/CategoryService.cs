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
        Task<Result<List<Category>>> GetCategory();
        Task<Result<Category>> GetCategory(Guid id);
        Task<Result<Category>> PutCategory(Guid id, Category category);
        Task<Result<Category>> PostCategory(Category category);
        Task<Result<Category>> DeleteCategory(Guid id);
    }

    public class CategoryService: ICategoryService
    {
        private readonly ShopContext _context;
        public CategoryService(ShopContext context)
        {
            _context = context;
        }

        public async Task<Result<Category>> DeleteCategory(Guid id)
        {
            var category = await _context.Category.SingleOrDefaultAsync(c=>c.CategoryId==id);
            if (category == null)
            {
                return new Result<Category>
                {
                    Success = false,
                    ResponseMessage = "There is no category with this id "
                };
            }
            try
            {
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new Result<Category>
                {
                    Success = false,
                    ResponseMessage = ex.Message
                };
            }
            return new Result<Category>
            {
                Success = true,
                ResponseMessage = "Category Deleted Successfully",
                ResponseObject = category
            };
        }

        public async Task<Result<List<Category>>> GetCategory()
        {
            List<Category> categories = await _context.Category.ToListAsync();
            return new Result<List<Category>>
            {
                Success = true,
                ResponseObject = categories,
            };
        }

        public async Task<Result<Category>> GetCategory(Guid id)
        {
            Category category = await _context.Category.SingleOrDefaultAsync(c=>c.CategoryId==id);

            if (category == null)
            {
                return new Result<Category>
                {
                    ResponseMessage = "Sorry There is no category with this id",
                    Success = false,
                };
            }

            return new Result<Category>
            {
                ResponseObject = category,
                Success = true,
            };
        }

        public async Task<Result<Category>> PostCategory(Category category)
        {
            category.CategoryId = Guid.NewGuid();
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return new Result<Category>
            {
                ResponseMessage = "Category added successfully",
                Success = true,
                ResponseObject=category
            };
        }

        public async Task<Result<Category>> PutCategory(Guid id, Category category)
        {
            try
            {
                var categoryData = _context.Category.SingleOrDefault(c => c.CategoryId == id);
                categoryData.Name = category.Name;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new Result<Category>
                {
                    Success = false,
                    ResponseMessage = ex.Message,
                };
            }
            return new Result<Category>
            {
                Success = true,
                ResponseMessage = "Category Update Successfuly",
            };
        }
    }
}
