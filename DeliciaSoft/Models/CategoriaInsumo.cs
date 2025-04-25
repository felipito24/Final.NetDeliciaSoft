using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class CategoriaInsumo
{
    public int IdCategoriaInsumos { get; set; }

    public string? NombreCategoria { get; set; }

    public string? Descripcion { get; set; }

    public DateOnly? FechaRegistro { get; set; }

    public int? CantidadInsumos { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Insumo> Insumos { get; set; } = new List<Insumo>();
}
