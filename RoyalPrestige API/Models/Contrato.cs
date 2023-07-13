using System;
using System.Collections.Generic;

namespace RoyalPrestige_API.Models;

public partial class Contrato
{
    public long Id { get; set; }

    public string Contrato1 { get; set; } = null!;

    public long ClienteId { get; set; }

    public long VendedorId { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Usuario Vendedor { get; set; } = null!;
}
