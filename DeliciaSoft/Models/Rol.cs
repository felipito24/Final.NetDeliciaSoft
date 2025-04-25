using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string? Rol1 { get; set; }

    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
