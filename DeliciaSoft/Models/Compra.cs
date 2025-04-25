using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public int? IdProveedor { get; set; }

    public DateOnly? FechaCompra { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public string? MetodoPago { get; set; }

    public string? Observaciones { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Iva { get; set; }

    public decimal? Total { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Proveedor? IdProveedorNavigation { get; set; }
}
