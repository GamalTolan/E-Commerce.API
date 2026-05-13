using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Attributes
{
    public class RedisCashAttribute(int durationinSocunds = 60) :ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cashService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CashService;
            var cashKey = GetCashKey(context.HttpContext.Request);
            var cashValue = await cashService.GetAsync(cashKey);
            if (cashValue is not null)
            {
                
                context.Result = new ContentResult
                {
                    Content = cashValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return ;
            }
            var executedContext = await next();
            if (executedContext.Result is ObjectResult objectResult)
              await cashService.SetAsync(cashKey, objectResult.Value, TimeSpan.FromSeconds(durationinSocunds));
        }
        
        private string GetCashKey(HttpRequest httpRequest)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(httpRequest.Path).Append("?"); 
            foreach (var item in httpRequest.Query.OrderBy(q => q.Key))
            {
                builder.Append($"{item.Key}={item.Value}$");

            }
            return builder.ToString().TrimEnd('$');
        }
    }
}
