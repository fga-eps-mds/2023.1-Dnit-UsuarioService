using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace app.Services
{
    public class AppController : ControllerBase
    {
        public ClaimsPrincipal? AppUsuario { get; set; }
        public ClaimsPrincipal Usuario => AppUsuario ?? User;
    }
}
