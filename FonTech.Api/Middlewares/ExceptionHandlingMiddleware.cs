using FonTech.Domain.Enum;
using FonTech.Domain.Result;
using System.Net;

namespace FonTech.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;
        public ExceptionHandlingMiddleware (RequestDelegate next, Serilog.ILogger logger)
        {
            _next = next;
            _logger = logger;   
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
             try
            {
                await _next(httpContext);
            }
            catch (Exception exception) 
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception) 
        {
            _logger.Error(exception, exception.Message);

            var errormessage = exception.Message;
            var response = exception switch
            {
              UnauthorizedAccessException _ => new BaseResult() 
              { 
                  ErrorMessage = errormessage, 
                  ErrorCode = (int)HttpStatusCode.Unauthorized 
              },

              _ => new BaseResult() 
              {
                  ErrorMessage = "Internal server error. Please retry later",
                  ErrorCode = (int)HttpStatusCode.InternalServerError
              }
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode =(int)response.ErrorCode;
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }
}
