using System;
using System.Collections.Generic;

namespace BookManagement.BusinessObjects.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public string? UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public int? Status { get; set; }

    public decimal TotalPrice { get; set; }
    public string? RecipientName { get; set; } = null!;
    public string? ShippingAddress { get; set; } = null!;
    public string? PhoneNumber { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? User { get; set; }
}
