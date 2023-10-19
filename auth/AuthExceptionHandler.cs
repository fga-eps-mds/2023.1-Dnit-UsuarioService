using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace auth
{
    public class AuthExceptionHandler : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is AuthException apiException)
            {
                context.Result = new JsonResult(new { Details = apiException.Message })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
