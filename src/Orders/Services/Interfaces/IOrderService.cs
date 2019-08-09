using Orders.Model.DTO.Order;
using Orders.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> FindOrderAsync(int id);
        Task<IEnumerable<Order>> GetOrdersAsync(SearchQuery options, int page, int limit);
        Task<Order> UpdateOrderAsync(Order Order);
        Task DeleteOrderAsync(Order Order);
    }
}
