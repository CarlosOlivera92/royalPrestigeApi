using System;
using System.Collections.Generic;

namespace RoyalPrestige_API.Models;

public partial class Demostracione
{
    public long Id { get; set; }

    public DateOnly FechaDemostracion { get; set; }

    public long ClienteId { get; set; }

    public long EstadoVisitaId { get; set; }

    public long EstadoVenta { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual EstadosDemostracion EstadoVentaNavigation { get; set; } = null!;

    public virtual EstadosDemostracion EstadoVisita { get; set; } = null!;

    public virtual ICollection<Reprogramacione> Reprogramaciones { get; set; } = new List<Reprogramacione>();
}
