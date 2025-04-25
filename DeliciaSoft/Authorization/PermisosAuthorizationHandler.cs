// PermisosAuthorizationHandler.cs
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace DeliciaSoft.Authorization
{
    public class PermisosAuthorizationHandler : AuthorizationHandler<PermisosRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermisosRequirement requirement)
        {
            // Verificar si el usuario tiene el permiso requerido
            if (context.User.HasClaim(c =>
                c.Type == "Permission" &&
                c.Value == requirement.PermisoRequerido))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}