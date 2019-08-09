using LinqKit;
using Microsoft.EntityFrameworkCore;
using Orders.Model.Context;
using Orders.Model.DTO.Order;
using Orders.Model.Entities;
using Orders.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orders.Utils;

namespace Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrdersDbContext _dbContext;

        public OrderService(OrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }

        public async Task DeleteOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Order> FindOrderAsync(int id)
        {
            return await _dbContext.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(SearchQuery options, int page, int limit)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var predicate = PredicateBuilder.New<Order>();

            predicate = PredicateBuilder.Or<Order>(predicate, order =>
                (string.IsNullOrEmpty(options.Address) || order.Address.Contains(options.Address)) &&
                (options.IsCallbackRequired == null || order.IsCallbackRequired == options.IsCallbackRequired) &&
                (options.MinimumAmount == null || order.Amount >= options.MinimumAmount) &&
                (options.MaximumAmount == null || order.Amount <= options.MaximumAmount) &&
                (options.Fruit == null || order.Fruit == options.Fruit) &&
                (options.FromETA == null || order.ETA >= options.FromETA) &&
                (options.UntilETA == null || order.ETA <= options.UntilETA)
            );

            IQueryable<Order> query = _dbContext.Orders
                .Where(predicate);

            if (!string.IsNullOrWhiteSpace(options.OrderBy) && typeof(Order).GetProperty(options.OrderBy) != null)
                if (options.Descending)
                    query = query.OrderByDescending(options.OrderBy);
                else
                    query = query.OrderBy(options.OrderBy);
            else
                query = query.OrderByDescending(x => x.Id);

            var orders = await query
                .Skip(page * limit)
                .Take(limit)
                .ToArrayAsync();

            return orders;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }
    }
}
