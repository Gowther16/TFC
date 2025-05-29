using System;
using System.Collections.Generic;

namespace TFC.Models;

public partial class Order
{
    public decimal Id { get; set; }

    public string OrderCode { get; set; } = null!;

    public decimal? UserId { get; set; }

    public decimal? CustomerId { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual User? User { get; set; }
}
