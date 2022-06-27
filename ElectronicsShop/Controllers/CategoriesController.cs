using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronicsShop.Models;
using ElectronicsShop.DTOs;

namespace ElectronicsShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ShopContext _context;

        public CategoriesController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<Result<List<Category>>> GetCategory()
        {
            List<Category> categories = await _context.Category.ToListAsync();
            return new Result<List<Category>>
            {
                Success=true,
                ResponseObject=categories,
            };
        }

        [HttpGet("{id}")]
        public async Task<Result<Category>> GetCategory(Guid id)
        {
            Category category = await _context.Category.FindAsync(id);

            if (category == null)
            {
                return new Result<Category>
                {
                    ResponseMessage = "Sorry There is no category with this id",
                    Success=false,
                };
            }

            return new Result<Category>
            {
                ResponseObject=category,
                Success=true,
            };
        }
        
        [HttpPut("{id}")]
        public async Task<Result<Category>> PutCategory(Guid id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return new Result<Category>
                {
                    Success = false,
                    ResponseMessage= "Enter correct data and try again"
                };
            }
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

        [HttpPost]
        public async Task<Result<Category>> PostCategory(Category category)
        {
            if(!ModelState.IsValid)
            {
                return new Result<Category>
                {
                    ResponseMessage = "Enter Correct Data and try again",
                    Success=false
                };
            }
            category.CategoryId = Guid.NewGuid();
            _context.Category.Add(category);
            await _context.SaveChangesAsync();

            return new Result<Category>
            {
                ResponseMessage = "Category added successfully",
                Success = true
            };
        }

        [HttpDelete("{id}")]
        public async Task<Result<Category>> DeleteCategory(Guid id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return new Result<Category>
                {
                    Success = false,
                    ResponseMessage="There is no category with this id "
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
                ResponseObject=category
            };
        }

        
    }
}
