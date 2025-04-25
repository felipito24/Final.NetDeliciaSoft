using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class ProduccionEstado
{
    public int? IdProduccion { get; set; }

    public int? IdEstado { get; set; }

    public virtual EstadoProduccion? IdEstadoNavigation { get; set; }

    public virtual Produccion? IdProduccionNavigation { get; set; }
}
