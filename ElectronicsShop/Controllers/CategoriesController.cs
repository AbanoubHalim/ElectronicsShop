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
using Microsoft.AspNetCore.Authorization;

namespace ElectronicsShop.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet("GetCategory")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            var categories = await categoryService.GetCategory();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound("There is no category with this id");
            }
            return category;
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(Guid id, Category category)
        {
            var categorydata = await categoryService.GetCategory(id);
            if (categorydata == null)
            {
                return NotFound("Sorry this category is not exist");
            }
            else if (id != category.CategoryId)
            {
                return BadRequest("Id must be with Same value");
            }
            await categoryService.PutCategory(id, category);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            else
            {
                await categoryService.PostCategory(category);
                return Ok(category);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await categoryService.GetCategory(id);
            if (category == null)
            {
                return NotFound("Sorry this category is not exist");
            }
            await categoryService.DeleteCategory(category);
            return NoContent();
        }
    }
}
