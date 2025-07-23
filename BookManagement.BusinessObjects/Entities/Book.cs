using System;
using System.Collections.Generic;
using BookManagement.BusinessObjects.Enum;

namespace BookManagement.BusinessObjects.Entities;

public partial class Book
{
    public int BookId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int Stock { get; set; }
    public int? Sold { get; set; }

    public int? CategoryId { get; set; }

    public string? Author { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public string? ImageUrl { get; set; }

    public BookStatus? Status { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Category { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? UpdatedByNavigation { get; set; }
}
