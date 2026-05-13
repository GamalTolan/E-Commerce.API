using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;
using System.Net;

namespace E_Commerce.API.Factories
{
    public class ApiResponseFactory
    {
        public static ActionResult CustomValidationErrorResponse(ActionContext context)
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Any())
                .Select(e => new ValidationError
                {
                    Key = e.Key,
                    Errors = e.Value.Errors.Select(x => x.ErrorMessage)
                });
            var validationResponse = new ValidationErrorResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = "Validation Failed.",
                Errors = errors
            };
            return new BadRequestObjectResult(validationResponse);

        }
    }
}
