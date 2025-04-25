using DeliciaSoft.Models;

public partial class RolPermiso
{
    public int IdRolPermiso { get; set; }

    public int? IdRol { get; set; }

    public int? IdPermiso { get; set; }

    public bool? Estado { get; set; }

    public virtual Permiso? IdPermisoNavigation { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
