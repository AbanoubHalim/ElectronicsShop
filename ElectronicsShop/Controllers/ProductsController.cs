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
using ElectronicsShop.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ElectronicsShop.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(int pageIndex = 1)
        {
            IEnumerable<Product> products= await productService.GetProduct();
            if (products.Any())
            {
                const int pagesize = 5;
                if (pageIndex < 1)
                    pageIndex = 1;

                var pager = new Pager(products.Count(), pageIndex, pagesize);
                int recSkip = (pageIndex - 1) * pagesize;

                var productssData = products.Skip(recSkip).Take(pager.PageSize).ToList();
                return Ok(productssData);
            }
            else
            {
                return Ok(products);
            }
             
        }

        [HttpGet("ProductsPerCategory/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> ProductsPerCategory(Guid id)
        {
            var products = await productService.ProductsPerCategory(id);
            if (!products.Any())
            {
                return NotFound("There is no product in this category");
            }
            return Ok(products);

             
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await productService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }

        [Authorize(Roles ="admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            var productdata = await productService.GetProduct(id);
            if (productdata == null)
            {
                return NotFound("Sorry this product is not exist");
            }
            else if (id != product.Id)
            {
                return BadRequest();
            }
            await productService.PutProduct(id, product);
            return NoContent();
        }


        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            else
            {
                await productService.PostProduct(product);
                return Ok(product);
            }

            
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await productService.GetProduct(id);
            if (product == null)
            {
                return NotFound("Sorry this product is not exist");
            }
            await productService.DeleteProduct(product);
            return NoContent();
        }

       
    }
}
