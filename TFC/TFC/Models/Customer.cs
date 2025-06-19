using System;
using System.Collections.Generic;

namespace TFC.Models;

public partial class Customer
{
    public decimal Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
