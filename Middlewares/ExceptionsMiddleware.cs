using System.Net;
using AsadTutorialWebAPI.Error;

namespace AsadTutorialWebAPI.Middlewares
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate next;

        public ILogger<ExceptionsMiddleware> logger { get; }
        public IHostEnvironment env { get; }

        public ExceptionsMiddleware(RequestDelegate next, ILogger<ExceptionsMiddleware> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                APIError response;
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                string Message = string.Empty;
                var exceptionType = ex.GetType();

                if (exceptionType == typeof(UnauthorizedAccessException))
                {
                    statusCode = HttpStatusCode.Unauthorized;
                    Message = "You are not authorized.";
                }
                else
                {
                    statusCode = HttpStatusCode.InternalServerError;
                    Message = "Some unknown error occurred.";
                }

                if (env.IsDevelopment())
                {
                    response = new APIError((int)statusCode, ex.Message, ex.StackTrace.ToString());
                }
                else
                {
                    response = new APIError((int)statusCode, Message);
                }

                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(response.ToString());
            }
        }
    }
}