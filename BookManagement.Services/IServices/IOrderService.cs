using BookManagement.BusinessObjects.Commons;
using BookManagement.BusinessObjects.Entities;
using BookManagement.Services.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookManagement.Services.IServices
{
    public interface IOrderService
    {
        Task AddOrderAsync(Order order);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<PagedResult<Order>> GetAllOrdersAsync(int pageNumber, int pageSize, string? recipientName, int? status);


    }
}
