using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class Permiso
{
    public int IdPermiso { get; set; }

    public string? Modulo { get; set; }

    public string? Accion { get; set; }

    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
}
