// PermisoAttribute.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace DeliciaSoft.Authorization
{
    public class PermisoAttribute : AuthorizeAttribute
    {
        public PermisoAttribute(string modulo, string accion)
        {
            Policy = $"{modulo}.{accion}";
        }
    }
}