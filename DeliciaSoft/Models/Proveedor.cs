using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string? TipoProveedor { get; set; }

    public string? Correo { get; set; }

    public string? Celular { get; set; }

    public string? Direccion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<PersonaJuridica> PersonaJuridicas { get; set; } = new List<PersonaJuridica>();

    public virtual ICollection<PersonaNatural> PersonaNaturals { get; set; } = new List<PersonaNatural>();
}
