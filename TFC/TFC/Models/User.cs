using System;
using System.Collections.Generic;

namespace TFC.Models;

public partial class User
{
    public decimal Id { get; set; }

    public string Name { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Role { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
