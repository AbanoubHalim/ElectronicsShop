using ElectronicsShop.DTOs;
using ElectronicsShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsShop.Services
{
    public interface IOrderService
    {
        Task<double> ConsumeCost(int piececount, Guid productId);
        Task<IEnumerable<Order>> GetOrder();
        Task<IEnumerable<Order>> GetUserOrders(Guid id);
        Task<Order> GetOrder(Guid id);
        Task PutOrder(Guid id, Order order);
        Task PostOrder(Order order);
        Task DeleteOrder(Order id);
    }
    public class OrderService:IOrderService
    {
        private readonly ShopContext _context;
        public OrderService(ShopContext context)
        {
            _context = context;
        }
        public async Task<double> ConsumeCost(int piececount, Guid productId)
        {
            Discount dis = await _context.Discount.FirstOrDefaultAsync(a => a.ProductId == productId);
            int Piececost = await _context.Product.Where(p => p.Id == productId)
                                     .Select(p => p.Price)
                                     .SingleOrDefaultAsync();
            if (dis == null)
            {
                return (piececount * piececount);
            }
            else if (piececount > 1)
            {
                return (piececount * piececount) - (dis.MoreOnePieceDiscountPercentage * (piececount * piececount));
            }
            else
            {
                return (piececount * piececount) - (dis.OnePieceDiscountPercentage * (piececount * piececount));
            }

        }

        public async Task DeleteOrder(Order order)
        {
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrder()
        {
            return await _context.Order.ToListAsync();    
        }

        public async Task<Order> GetOrder(Guid id)
        {
            return  await _context.Order.SingleOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetUserOrders(Guid id)
        {
            return await _context.Order.Where(o => o.UserId == id).ToListAsync();
        }

        public async Task PostOrder(Order order)
        {
            order.OrderId = Guid.NewGuid();
            _context.Order.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task PutOrder(Guid id, Order order)
        {
            var orderData = await _context.Order.SingleOrDefaultAsync(o => o.OrderId == id);           
            orderData.ProductId = order.ProductId;
            orderData.PieceCount = order.PieceCount;
            orderData.OrderCost = await ConsumeCost(order.PieceCount, order.ProductId);
            await _context.SaveChangesAsync();
        }

    }
}
