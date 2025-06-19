using System;
using System.Collections.Generic;

namespace TFC.Models;

public partial class Product
{
    public decimal Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public decimal? Inventory { get; set; }

    public decimal? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Comboitem> Comboitems { get; set; } = new List<Comboitem>();

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();
}
