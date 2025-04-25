using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class Ventum
{
    public int IdVenta { get; set; }

    public DateOnly? FechaVenta { get; set; }

    public string? Sede { get; set; }

    public int? IdCliente { get; set; }

    public string? MetodoPago { get; set; }

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();

    public virtual Cliente? IdClienteNavigation { get; set; }
}
