using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class ProductoCompra
{
    public int IdProducto { get; set; }

    public string? NombreProducto { get; set; }

    public string? UnidadMedida { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual ICollection<DetalleVentum> DetalleVenta { get; set; } = new List<DetalleVentum>();
}
