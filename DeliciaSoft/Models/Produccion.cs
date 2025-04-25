using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class Produccion
{
    public int IdProduccion { get; set; }

    public string? ProductoAelaborar { get; set; }

    public DateOnly? FechaPedido { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    public string? NumeroPedido { get; set; }
}
