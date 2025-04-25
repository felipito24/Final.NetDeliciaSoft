// PermisosRequirement.cs
using Microsoft.AspNetCore.Authorization;

namespace DeliciaSoft.Authorization
{
    public class PermisosRequirement : IAuthorizationRequirement
    {
        public string PermisoRequerido { get; }

        public PermisosRequirement(string permisoRequerido)
        {
            PermisoRequerido = permisoRequerido;
        }
    }
}