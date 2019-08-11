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

        public async Task<IEnumerable<Order>> GetOrdersAsync(SearchRequest request, int page, int limit)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var query = BuildSearchQuery(request);

            var orders = await query
                .Skip(page * limit)
                .Take(limit)
                .ToArrayAsync();

            return orders;
        }

        public async Task<int> GetOrdersCountAsync(SearchRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var query = BuildSearchQuery(request);

            var totalItems = await query.CountAsync();

            return totalItems;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }

        private IQueryable<Order> BuildSearchQuery(SearchRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var predicate = PredicateBuilder.New<Order>();

            predicate = PredicateBuilder.Or<Order>(predicate, order =>
                (string.IsNullOrEmpty(request.Address) || order.Address.Contains(request.Address)) &&
                (request.IsCallbackRequired == null || order.IsCallbackRequired == request.IsCallbackRequired) &&
                (request.MinimumAmount == null || order.Amount >= request.MinimumAmount) &&
                (request.MaximumAmount == null || order.Amount <= request.MaximumAmount) &&
                (request.Fruits == null || request.Fruits.Contains(order.Fruit)) &&
                (request.FromETA == null || order.ETA >= request.FromETA) &&
                (request.UntilETA == null || order.ETA <= request.UntilETA)
            );

            IQueryable<Order> query = _dbContext.Orders
                .Where(predicate);

            if (!string.IsNullOrWhiteSpace(request.OrderBy) && typeof(Order).GetProperty(request.OrderBy) != null)
                if (request.Descending)
                    query = query.OrderByDescending(request.OrderBy);
                else
                    query = query.OrderBy(request.OrderBy);
            else
                query = query.OrderByDescending(x => x.Id);

            return query;
        }
    }
}
