using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class Insumo
{
    public int IdInsumo { get; set; }

    public string? NombreInsumo { get; set; }

    public int? IdCategoriaInsumos { get; set; }

    public decimal? Cantidad { get; set; }

    public string? UnidadMedida { get; set; }

    public string? Marca { get; set; }

    public string? Imagen { get; set; }

    public bool? Estado { get; set; }

    public virtual CategoriaInsumo? IdCategoriaInsumosNavigation { get; set; }

    public virtual ICollection<ProductoPersonalizado> ProductoPersonalizados { get; set; } = new List<ProductoPersonalizado>();
}

