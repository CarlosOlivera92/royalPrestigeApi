using System;
using System.Collections.Generic;

namespace RoyalPrestige_API.Models;

public partial class Vendedores
{
    public long Id { get; set; }

    public long VendedorId { get; set; }

    public virtual Usuario? Vendedor { get; set; }
}
