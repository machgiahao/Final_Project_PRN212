using System;
using System.Collections.Generic;

namespace BookManagement.BusinessObjects.Entities;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? Email { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Role { get; set; }

    public string? Bio { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Book> BookCreatedByNavigations { get; set; } = new List<Book>();

    public virtual ICollection<Book> BookUpdatedByNavigations { get; set; } = new List<Book>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
