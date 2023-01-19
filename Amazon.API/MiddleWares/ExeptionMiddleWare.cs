using Amazon.API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Amazon.API.MiddleWares
{
    public class ExeptionMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExeptionMiddleWare> logger;
        private readonly IHostEnvironment environment;

        public ExeptionMiddleWare(RequestDelegate next, ILogger<ExeptionMiddleWare> logger, IHostEnvironment environment)
        {
            this.next = next;
            this.logger = logger;
            this.environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context); // Move to Next MiddleWare if No Exeptions
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var errorResponse = environment.IsDevelopment() ?
                    new ApiExeptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    :
                    new ApiExeptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString());

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(errorResponse, options);

                await context.Response.WriteAsync(json);
            }

        }

    }
}
