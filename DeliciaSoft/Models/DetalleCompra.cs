using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class DetalleCompra
{
    public int IdDetalle { get; set; }

    public int? IdCompra { get; set; }

    public int? IdProducto { get; set; }

    public decimal? Cantidad { get; set; }

    public string? UnidadMedida { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual ProductoCompra? IdProductoNavigation { get; set; }
}
