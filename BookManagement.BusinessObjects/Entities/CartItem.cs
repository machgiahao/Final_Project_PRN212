using System;
using System.Collections.Generic;

namespace BookManagement.BusinessObjects.Entities;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public string? UserId { get; set; }

    public int? BookId { get; set; }

    public int Quantity { get; set; }
    public DateTime? CreatedAt { get; set; }
    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}
