using System;
using System.Collections.Generic;

namespace RoyalPrestige_API.Models;

public partial class EstadosDemostracion
{
    public long Id { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Demostracione> DemostracioneEstadoVentaNavigations { get; set; } = new List<Demostracione>();

    public virtual ICollection<Demostracione> DemostracioneEstadoVisita { get; set; } = new List<Demostracione>();
}
