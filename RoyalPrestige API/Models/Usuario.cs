using System;
using System.Collections.Generic;

namespace RoyalPrestige_API.Models;

public partial class Usuario
{
    public long Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? ProfilePic { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public long Telefono { get; set; }

    public long RolId { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

    public virtual Role? Rol { get; set; }

    public virtual ICollection<Vendedores> Vendedores { get; set; } = new List<Vendedores>();
}
