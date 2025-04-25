using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class DetalleProductoGeneral
{
    public int IdDetalleGeneral { get; set; }

    public int? IdProductoGeneral { get; set; }

    public decimal? Cantidad { get; set; }

    public string? UnidadMedida { get; set; }

    public virtual ProductoGeneral? IdProductoGeneralNavigation { get; set; }
}
