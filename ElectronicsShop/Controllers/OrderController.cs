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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrder()
        {
            return Ok(await orderService.GetOrder());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUserOrders/{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders(Guid id)
        {
            var orders = await orderService.GetUserOrders(id);
            if (!orders.Any())
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            var order = await orderService.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, Order order)
        {
            var orderdata = await orderService.GetOrder(id);
            if (orderdata == null)
            {
                return NotFound("Sorry this order is not exist");
            }
            else if (id != order.OrderId)
            {
                return BadRequest();
            }          
            await orderService.PutOrder(id, order);
            return NoContent();
        }
        
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            else
            {
                await orderService.PostOrder(order);
                return Ok(order);
            }            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order= await orderService.GetOrder(id);
            if (order == null)
            {
                return NotFound("Sorry this order is not exist");
            }
            await orderService.DeleteOrder(order);
            return NoContent();
        }
        
    }
}
