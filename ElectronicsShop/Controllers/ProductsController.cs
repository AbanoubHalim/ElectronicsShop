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
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _context;

        public ProductsController(ShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<Result<List<Product>>> GetProduct()
        {
            List<Product> allProducts = await _context.Product.ToListAsync();
            return new Result<List<Product>>
            {
                Success = true,
                ResponseObject = allProducts,
            };
        }
        [HttpGet("ProductsPerCategory/{id}")]
        public async Task<Result<List<Product>>> ProductsPerCategory(Guid id)
        {
            List<Product> allProducts = await _context.Product.Where(p=>p.CategoryId==id).ToListAsync();
            return new Result<List<Product>>
            {
                Success = true,
                ResponseObject = allProducts,
            };
        }

        [HttpGet("{id}")]
        public async Task<Result<Product>> GetProduct(Guid id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return new Result<Product>
                {
                    Success = false,
                    ResponseMessage = "There is no product with this id"
                };
            }

            return new Result<Product>
            {
                Success = true,
                ResponseObject=product
            };
        }

        [HttpPut("{id}")]
        public async Task<Result<Product>> PutProduct(Guid id, Product product)
        {
            var productData = await _context.Product.SingleOrDefaultAsync(p => p.Id == id);
            try
            {
            if(productData==null)
            {
                return new Result<Product>
                {
                    Success = false,
                    ResponseMessage = "There is no product for this id"
                };
            }
            productData.Name = product.Name;
            productData.PieceCount = product.PieceCount;
            productData.Price = product.Price;
            productData.Description = product.Description;
            productData.CategoryId = product.CategoryId;
            
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new Result<Product>
                {
                    Success=false,
                    ResponseMessage=ex.Message
                };
            }

            return new Result<Product>
            {
                Success = true,
                ResponseMessage = "Product Updated Successfully",
                ResponseObject=productData,
            };
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
            product.Id = Guid.NewGuid();
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return new Result<Product>
            {
                Success = true,
                ResponseMessage = "Product added Successfully",
            };
        }

        [HttpDelete("{id}")]
        public async Task<Result<Product>> DeleteProduct(Guid id)
        {
            var product = _context.Product.SingleOrDefault(p=>p.Id==id);
            if (product == null)
            {
                return new Result<Product>
                {
                    Success = false,
                    ResponseMessage = "There is no product with this  "
                };
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return new Result<Product>
            {
                Success = true,
                ResponseObject = product,
                ResponseMessage = "Product deleted Successfully"
            };
        }

       
    }
}
