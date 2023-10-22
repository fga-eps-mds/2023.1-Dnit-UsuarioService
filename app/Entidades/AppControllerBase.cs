using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace app.Entidades
{
    public class AppControllerBase : ControllerBase
    {
        public ClaimsPrincipal? AppUsuario { get; set; }

        public ClaimsPrincipal Usuario => AppUsuario ?? User;
    }
}
