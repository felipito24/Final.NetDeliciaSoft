using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class DetallePedido
{
    public int IdDetallePedido { get; set; }

    public int? IdPedido { get; set; }

    public int? IdProducto { get; set; }

    public int? TotalProductos { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? Iva { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Total { get; set; }

    public virtual Pedido? IdPedidoNavigation { get; set; }

    public virtual ProductoCompra? IdProductoNavigation { get; set; }
}
