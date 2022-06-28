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
        Task<Result<List<Order>>> GetOrder();
        Task<Result<List<Order>>> GetUSerOrders(Guid id);
        Task<Result<Order>> GetOrder(Guid id);
        Task<Result<Order>> PutOrder(Guid id, Order order);
        Task<Result<Order>> PostOrder(Order order);
        Task<Result<Order>> DeleteOrder(Guid id);
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

        public async Task<Result<Order>> DeleteOrder(Guid id)
        {
            var order = await _context.Order.SingleOrDefaultAsync(a => a.OrderId == id);
            if (order == null)
            {
                return new Result<Order>
                {
                    Success = false,
                    ResponseMessage = "There is no order with this id"
                };
            }
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return new Result<Order>
            {
                Success = true,
                ResponseMessage = "Order Deleted Successfully"
            };
        }

        public async Task<Result<List<Order>>> GetOrder()
        {
            List<Order> allOrders = await _context.Order.ToListAsync();
            return new Result<List<Order>>
            {
                ResponseObject = allOrders,
                Success = true
            };
        }

        public async Task<Result<Order>> GetOrder(Guid id)
        {
            var order = await _context.Order.SingleOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return new Result<Order>
                {
                    Success = false,
                    ResponseMessage = "There is no order with this id"
                };
            }

            return new Result<Order>
            {
                Success = true,
                ResponseObject = order
            };
        }

        public async Task<Result<List<Order>>> GetUSerOrders(Guid id)
        {
            List<Order> orders = await _context.Order.Where(o => o.UserId == id).ToListAsync();

            return new Result<List<Order>>
            {
                Success = true,
                ResponseObject = orders
            };
        }

        public async Task<Result<Order>> PostOrder(Order order)
        {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            return new Result<Order>
            {
                Success = true,
                ResponseObject = order,
                ResponseMessage="Order Added Successfully"
            };
        }

        public async Task<Result<Order>> PutOrder(Guid id, Order order)
        {
            var orderData = await _context.Order.SingleOrDefaultAsync(o => o.OrderId == id);
            if (orderData == null)
            {
                return new Result<Order>
                {
                    Success = false,
                    ResponseMessage = "There is no order with this id"
                };
            }
            orderData.ProductId = order.ProductId;
            orderData.PieceCount = order.PieceCount;
            orderData.OrderCost = await ConsumeCost(order.PieceCount, order.ProductId);

            await _context.SaveChangesAsync();
            return new Result<Order>
            {
                Success = true,
                ResponseObject = orderData
            };
        }
    }
}
