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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }


        [HttpGet]
        public async Task<Result<List<Product>>> GetProduct()
        {
            return await productService.GetProduct();
        }
        [HttpGet("ProductsPerCategory/{id}")]
        public async Task<Result<List<Product>>> ProductsPerCategory(Guid id)
        {
            return await productService.ProductsPerCategory(id);
        }

        [HttpGet("{id}")]
        public async Task<Result<Product>> GetProduct(Guid id)
        {
            return await productService.GetProduct(id);
        }

        [HttpPut("{id}")]
        public async Task<Result<Product>> PutProduct(Guid id, Product product)
        {
            return await productService.PutProduct(id, product);
        }

        [HttpPost]
        public async Task<Result<Product>> PostProduct(Product product)
        {
            if(!ModelState.IsValid)
            {
                return new Result<Product>
                {
                    Success=false,
                    ResponseMessage="Enter correct data and try again",
                };
            }
            return await productService.PostProduct(product);
        }

        [HttpDelete("{id}")]
        public async Task<Result<Product>> DeleteProduct(Guid id)
        {
            return await productService.DeleteProduct(id);
        }

       
    }
}
