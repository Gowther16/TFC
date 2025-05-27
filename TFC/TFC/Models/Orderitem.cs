using System;
using System.Collections.Generic;

namespace TFC.Models;

public partial class Orderitem
{
    public decimal Id { get; set; }

    public decimal OrderId { get; set; }

    public decimal? ProductId { get; set; }

    public decimal? ComboId { get; set; }

    public decimal? Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Combo? Combo { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product? Product { get; set; }
}
