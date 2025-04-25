using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class DetalleProduccion
{
    public int IdDetalleProduccion { get; set; }

    public int? IdProductoGeneral { get; set; }

    public int? IdProductoPersonalizado { get; set; }

    public decimal? CantidadProducto { get; set; }

    public virtual ProductoGeneral? IdProductoGeneralNavigation { get; set; }

    public virtual ProductoPersonalizado? IdProductoPersonalizadoNavigation { get; set; }
}
