using System;
using System.Collections.Generic;

namespace TFC.Models;

public partial class Category
{
    public decimal Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
