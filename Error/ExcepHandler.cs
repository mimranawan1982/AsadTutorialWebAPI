using System.Net;
using AsadTutorialWebAPI.Error;
using AsadTutorialWebAPI.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AsadTutorialWebAPI.ExceptionHandler
{
    public class ExcepHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if (exception is not NotImplementedException)
            {
                var response = new APIError
                {

                    ErrorCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = exception.StackTrace.ToString(),
                    ErrorDetails = "Something went wrong"
                };
                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            else if (exception is NotImplementedException)
            {
                var response = new APIError
                {
                    ErrorCode = (int)HttpStatusCode.NotImplemented,
                    ErrorMessage = exception.StackTrace.ToString(),
                    ErrorDetails = "Something went wrong"
                };
                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
                httpContext.Response.StatusCode = (int) HttpStatusCode.NotImplemented;
            }

            return true;
        }
    }
}
