using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectronicsShop.Models;
using ElectronicsShop.DTOs;
using ElectronicsShop.Services;

namespace ElectronicsShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<Result<List<Category>>> GetCategory()
        {
            return await categoryService.GetCategory();
        }

        [HttpGet("{id}")]
        public async Task<Result<Category>> GetCategory(Guid id)
        {
            return await categoryService.GetCategory(id);
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
            return await categoryService.PutCategory(id, category);
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
            return await categoryService.PostCategory(category);
        }

        [HttpDelete("{id}")]
        public async Task<Result<Category>> DeleteCategory(Guid id)
        {
            return await categoryService.DeleteCategory(id);
        }

        
    }
}
