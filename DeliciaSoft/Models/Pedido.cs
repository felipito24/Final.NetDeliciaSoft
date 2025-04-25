using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int? IdCliente { get; set; }

    public string? MetodoPago { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    public DateOnly? FechaPedido { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Cliente? IdClienteNavigation { get; set; }
}
