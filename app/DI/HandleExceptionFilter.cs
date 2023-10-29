using app.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography;

namespace app.DI
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> logger;

        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ApiException apiException)
            {
                logger.LogWarning(apiException, "An API Exception was caught");

                context.Result = new JsonResult(apiException.Error, JsonConvert.DefaultSettings)
                {
                    StatusCode = (int)HttpStatusCode.UnprocessableEntity,
                };
                context.ExceptionHandled = true;
            }
            else
            {
                var rawCode = new byte[3];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(rawCode);
                }
                var code = BitConverter.ToString(rawCode).Replace("-", "");

                logger.LogError(context.Exception, "Unhandled exception -- code: {ExceptionCode}", code);

                var error = new
                {
                    Message = $"Um erro inesperado aconteceu. Contate o administrador do sistema e informe o código: {code}",
                    ExceptionCode = code,
                };

                context.Result = new JsonResult(error, JsonConvert.DefaultSettings)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
