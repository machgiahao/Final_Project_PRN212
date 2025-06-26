using BookManagement.BusinessObjects.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.DataAccess.IRepositories
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}
