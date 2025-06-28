using Microsoft.EntityFrameworkCore;
using BookManagement.DataAccess.Context;
using BookManagement.BusinessObjects.Entities;
using BookManagement.DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagement.DataAccess.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly BookStoreDbContext _context;

        public OrderDetailRepository(BookStoreDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                await _context.OrderDetails.AddAsync(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while adding the order detail to the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the order detail.", ex);
            }
        }

        public async Task<OrderDetail?> GetOrderDetailByIdAsync(int id)
        {
            try
            {
                return await _context.OrderDetails
                    .Include(od => od.Book)
                    .Include(od => od.Order)
                    .FirstOrDefaultAsync(od => od.OrderDetailId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the order detail with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            try
            {
                return await _context.OrderDetails
                    .Include(od => od.Book)
                    .Where(od => od.OrderId == orderId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving order details for order ID {orderId}.", ex);
            }
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailsAsync()
        {
            try
            {
                return await _context.OrderDetails
                    .Include(od => od.Book)
                    .Include(od => od.Order)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all order details.", ex);
            }
        }

        public async Task UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            try
            {
                _context.OrderDetails.Update(orderDetail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("A concurrency error occurred while updating the order detail.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating the order detail in the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the order detail.", ex);
            }
        }

        public async Task DeleteOrderDetailAsync(int id)
        {
            try
            {
                var orderDetail = await _context.OrderDetails.FindAsync(id);
                if (orderDetail != null)
                {
                    _context.OrderDetails.Remove(orderDetail);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new KeyNotFoundException($"Order detail with ID {id} not found.");
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while deleting the order detail from the database.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the order detail.", ex);
            }
        }
    }
}
