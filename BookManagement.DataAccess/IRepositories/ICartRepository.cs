﻿using BookManagement.BusinessObjects.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagement.DataAccess.IRepositories
{
    public interface ICartRepository
    {
        Task AddCartItemAsync(CartItem cartItem);
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
        Task<IEnumerable<CartItem>> GetCartItemsByUserIdAsync(string userId);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int cartItemId);
        Task RemoveCartItemsAsync(string userId, List<int> cartItemIds);
        Task ClearCartAsync(string userId);
        public Task<int> GetCartCountAsync(string userId);
        public Task<CartItem?> GetCartItemByBookIdAsync(int bookId, string userId);
        Task DeleteSoldOutItems(List<int> itemIds);
    }
}
