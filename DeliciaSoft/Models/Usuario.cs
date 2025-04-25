using System;
using System.Collections.Generic;

namespace DeliciaSoft.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Correo { get; set; }

    public string? Contrasena { get; set; }

    public string? Direccion { get; set; }

    public string? Barrio { get; set; }

    public string? Ciudad { get; set; }

    public string? TipoDocumento { get; set; }

    public string? NumeroDocumento { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Celular { get; set; }

    public int? IdRol { get; set; }

    public bool? Estado { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
