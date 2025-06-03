using System;
using System.Collections.Generic;

namespace TFC.Models;

public partial class Comboitem
{
    public decimal ComboId { get; set; }

    public decimal ProductId { get; set; }

    public decimal? Quantity { get; set; }

    public virtual Combo Combo { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
