using System;
using System.Collections.Generic;

namespace RoyalPrestige_API.Models;

public partial class Cliente
{
    public long Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public long Telefono { get; set; }

    public string Direccion { get; set; } = null!;

    public string? Observaciones { get; set; }

    public string Email { get; set; } = null!;

    public long VendedorId { get; set; }

    public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

    public virtual ICollection<Demostracione> Demostraciones { get; set; } = new List<Demostracione>();

    public virtual Usuario Vendedor { get; set; } = null!;
}
