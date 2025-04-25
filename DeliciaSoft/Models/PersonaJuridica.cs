using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class PersonaJuridica
{
    public int IdProveedorJuridico { get; set; }

    public string? NitEmpresa { get; set; }

    public string? NombreEmpresa { get; set; }

    public int? IdProveedor { get; set; }

    public virtual Proveedor? IdProveedorNavigation { get; set; }
}
