using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class ProductoGeneral
{
    public int IdProductoGeneral { get; set; }

    public string? NombreProducto { get; set; }

    public int? IdCategoriaProducto { get; set; }

    public decimal? PrecioProducto { get; set; }

    public decimal? CantidadProducto { get; set; }

    public string? Imagen { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<DetalleProduccion> DetalleProduccions { get; set; } = new List<DetalleProduccion>();

    public virtual ICollection<DetalleProductoGeneral> DetalleProductoGenerals { get; set; } = new List<DetalleProductoGeneral>();

    public virtual CategoriaProducto? IdCategoriaProductoNavigation { get; set; }

    public virtual ICollection<ProductoPersonalizado> ProductoPersonalizados { get; set; } = new List<ProductoPersonalizado>();
}
