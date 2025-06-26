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

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual User? User { get; set; }
}
