using Amazon.API.Errors;
using Amazon.API.Helpers;
using Amazon.Core.Repositories;
using Amazon.Core.Services;
using Amazon.Repository;
using Amazon.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Amazon.API.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices( this IServiceCollection services)
        {
            services.AddSingleton<IResponseCashService, ResponseCashService>();

            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IOrderServices, OrderServices>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITokenServices, TokenServices>();

            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAutoMapper(typeof(MappingProfiles));


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(M => M.Value.Errors.Count() > 0)
                                                         .SelectMany(M => M.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToArray();
                    var errorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);
                };
            });


            return services;
        }
    }
}
