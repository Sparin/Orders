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
        Task<IEnumerable<Order>> GetOrdersAsync(SearchRequest request, int page, int limit);
        Task<int> GetOrdersCountAsync(SearchRequest request);
        Task<Order> UpdateOrderAsync(Order Order);
        Task DeleteOrderAsync(Order Order);
    }
}
