using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class Sede
{
    public int IdSede { get; set; }

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Horarios { get; set; }

    public bool Estado { get; set; }
}
