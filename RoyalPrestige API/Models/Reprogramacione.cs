using System;
using System.Collections.Generic;

namespace RoyalPrestige_API.Models;

public partial class Reprogramacione
{
    public long Id { get; set; }

    public long DemostracionId { get; set; }

    public DateOnly NuevaFecha { get; set; }

    public string? Observaciones { get; set; }

    public virtual Demostracione Demostracion { get; set; } = null!;
}
