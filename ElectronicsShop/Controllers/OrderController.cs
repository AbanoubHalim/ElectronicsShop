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
    public class OrderController : ControllerBase
    {

        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<Result<List<Order>>> GetOrder()
        {
            return await orderService.GetOrder();
        }
        [HttpGet("GetUSerOrders/{id}")]
        public async Task<Result<List<Order>>> GetUSerOrders(Guid id)
        {
            return await orderService.GetUSerOrders(id);
        }
        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<Result<Order>> GetOrder(Guid id)
        {
            return await orderService.GetOrder(id);
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<Result<Order>> PutOrder(Guid id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return new Result<Order>
                {
                    Success = false,
                    ResponseMessage = "Enter correct Data and try again"
                };
            }
            else
                return await orderService.PutOrder(id, order);
        }

        
        [HttpPost]
        public async Task<Result<Order>> PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return new Result<Order>
                {
                    Success = false,
                    ResponseMessage = "Enter correct Data and try again"
                };
            }
            else return await orderService.PostOrder(order);
        }

        [HttpDelete("{id}")]
        public async Task<Result<Order>> DeleteOrder(Guid id)
        {
            return await orderService.DeleteOrder(id);
        }
   
    }
}
