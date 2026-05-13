using Domain.Exceptions;
using Shared.ErrorModels;
using System.ComponentModel.DataAnnotations;
using System.Net;
using ValidationException = Domain.Exceptions.ValidationException;

namespace E_Commerce.API.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;


        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }

        public async Task InvokeAsync(HttpContext httpcontext)
        {
            try
            {
                await _next(httpcontext);
                if (httpcontext.Response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    await HandleNotFoundEndPointAsync(httpcontext);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something Went Wrong {ex}");

                await HandleExceptionAsync(httpcontext, ex);
                 
            }
        }
         
        private async Task HandleExceptionAsync(HttpContext httpcontext, Exception exception)
        {

            httpcontext.Response.ContentType = "application/json";

            var errorDetails = new ErrorDetails()
            {

                ErrorMessage = exception.Message,

            };
            httpcontext.Response.StatusCode = exception switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnAuthorizedException => (int)HttpStatusCode.Unauthorized,
                ValidationException validationException => HandelValidationException(validationException, errorDetails),
                _ => (int)HttpStatusCode.InternalServerError
            };
            errorDetails.StatusCode = httpcontext.Response.StatusCode;

            await httpcontext.Response.WriteAsync(errorDetails.ToString());



        }
        private async Task HandleNotFoundEndPointAsync(HttpContext httpcontext)
        {

            httpcontext.Response.ContentType = "application/json";

            var errorDetails = new ErrorDetails()
            {
                ErrorMessage = $"The End Point{httpcontext.Request.Path} Not Found",
                StatusCode = (int)HttpStatusCode.NotFound
            }.ToString();


            await httpcontext.Response.WriteAsync(errorDetails);

        }

        private int HandelValidationException(ValidationException exception, ErrorDetails errorDetails)
        {
            errorDetails.Errors = exception.Errors;
            return (int)HttpStatusCode.BadRequest;
        }
    }
}
