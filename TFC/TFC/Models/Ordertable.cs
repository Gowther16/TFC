using System;
using System.Collections.Generic;

namespace TFC.Models;

public partial class Ordertable
{
    public decimal Id { get; set; }

    public string TableNumber { get; set; } = null!;

    public string? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
