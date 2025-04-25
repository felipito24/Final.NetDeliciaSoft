namespace DeliciaSoft.ViewModels.Permiso
{
    public class PermisoViewModel
    {
        public int IdPermiso { get; set; }
        public string Modulo { get; set; }
        public string Accion { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public bool Asignado { get; set; }
    }
}