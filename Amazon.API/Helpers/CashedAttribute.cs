using Amazon.Core.Services;
using Amazon.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom.Compiler;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Amazon.API.Helpers
{
    public class CashedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLiveInSecondes;

        public CashedAttribute(int timeToLiveInSecondes )
        {
            this.timeToLiveInSecondes = timeToLiveInSecondes;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cashService = context.HttpContext.RequestServices.GetRequiredService<IResponseCashService>();

            var cashKey = GenerateCashKeyFromRequest(context.HttpContext.Request);

            var cashedResponse = await cashService.GetCashedResponseAsync(cashKey);
            if (!string.IsNullOrEmpty(cashedResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cashedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            var executedEndPointContext = await next(); // Will Execute The Endpoint 
            if (executedEndPointContext.Result is OkObjectResult okObjectResult)
            {
                await cashService.CashResponseAsync(cashKey, okObjectResult.Value, TimeSpan.FromSeconds(timeToLiveInSecondes));
            }
        }

        private string GenerateCashKeyFromRequest(HttpRequest request)
        {
            var KeyByilder = new StringBuilder();
            KeyByilder.Append(request.Path);
            foreach (var (key , value) in request.Query.OrderBy(x => x.Key))
            {
                KeyByilder.Append($"|{key}-{value}");
            }
            return KeyByilder.ToString();

        }
    }
}
