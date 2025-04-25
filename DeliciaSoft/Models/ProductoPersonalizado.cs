using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class ProductoPersonalizado
{
    public int IdProductoPersonalizado { get; set; }

    public int? IdProducto { get; set; }

    public decimal? Cantidad { get; set; }

    public string? Descripcion { get; set; }

    public decimal? Total { get; set; }

    public string? Decoracion { get; set; }

    public string? TemasOmotivos { get; set; }

    public int? Toppings { get; set; }

    public string? Mensaje { get; set; }

    public string? DisenioPersonalizado { get; set; }

    public byte[]? ImagenReferencia { get; set; }

    public virtual ICollection<DetalleProduccion> DetalleProduccions { get; set; } = new List<DetalleProduccion>();

    public virtual ProductoGeneral? IdProductoNavigation { get; set; }

    public virtual Insumo? ToppingsNavigation { get; set; }
}
