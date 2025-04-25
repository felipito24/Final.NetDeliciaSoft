using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class CategoriaProducto
{
    public int IdCategoriaProducto { get; set; }

    public string? NombreCategoria { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public int? CantidadProducto { get; set; }

    public string? DescripcionProducto { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<ProductoGeneral> ProductoGenerals { get; set; } = new List<ProductoGeneral>();
}
