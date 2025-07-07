using Microsoft.EntityFrameworkCore;
using BookManagement.DataAccess.Context;
using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagement.BusinessObjects.Commons;

namespace BookManagement.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookStoreDbContext _context;

        public OrderRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while adding the order to the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the order.", ex);
            }
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Book)
                    .FirstOrDefaultAsync(o => o.OrderId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the order with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            try
            {
                return await _context.Orders
                    .OrderByDescending(o => o.OrderDate)
                    .Include(o => o.User)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Book)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all orders.", ex);
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Book)
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving orders for user ID {userId}.", ex);
            }
        }

        public async Task UpdateOrderAsync(Order order)
        {
            try
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("A concurrency error occurred while updating the order.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the order in the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the order.", ex);
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order != null)
                {
                    _context.Orders.Remove(order);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException($"Order with ID {id} not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the order from the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the order.", ex);
            }
        }

        public async Task<PagedResult<Order>> GetAllOrdersAsync(int pageNumber, int pageSize, string? recipientName, int? status)
        {
            try
            {
                var query = _context.Orders.AsQueryable();

                if (!string.IsNullOrEmpty(recipientName))
                {
                    query = query.Where(u => u.RecipientName.Contains(recipientName));
                }
                if (status != null)
                {
                    query = query.Where(u => u.Status == status);
                }

                var totalCount = await query.CountAsync();
                var orders = await query
                    .OrderByDescending(o => o.OrderDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return new PagedResult<Order>
                {
                    Items = orders,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all orders.", ex);
            }
        }
    }
}
