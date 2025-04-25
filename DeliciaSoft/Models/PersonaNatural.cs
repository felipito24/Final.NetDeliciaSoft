using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class PersonaNatural
{
    public int IdProveedorNatural { get; set; }

    public string? Documento { get; set; }

    public string? Nombre { get; set; }

    public string? Apellidos { get; set; }

    public int? IdProveedor { get; set; }

    public virtual Proveedor? IdProveedorNavigation { get; set; }
}
