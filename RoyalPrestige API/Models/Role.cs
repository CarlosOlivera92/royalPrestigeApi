using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoyalPrestige_API.Models;

public partial class Role
{
    [Key]
    public long Id { get; set; }

    public string Rol { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
