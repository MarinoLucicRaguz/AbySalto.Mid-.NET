using AbySalto.Mid.Application.Interfaces;
using AbySalto.Mid.Infrastructure.External.DummyJson;
using AbySalto.Mid.Infrastructure.External;
using AbySalto.Mid.Infrastructure.Options;
using AbySalto.Mid.Infrastructure.Persistence;
using AbySalto.Mid.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using Polly;
using Polly.Extensions.Http;

namespace AbySalto.Mid.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.AddHttpClient<IProductApi, DummyJsonApiClient>((sp, client) =>
                {
                    var opt = sp.GetRequiredService<IOptions<DummyJsonOptions>>().Value;
                    client.BaseAddress = new Uri(opt.BaseUrl);
                    client.Timeout = TimeSpan.FromSeconds(opt.TimeoutSeconds);
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddDatabase(configuration);
            services.AddServices();

            services.AddOptions<DummyJsonOptions>().Bind(configuration.GetSection("ExternalApis:DummyJson")).ValidateDataAnnotations().ValidateOnStart();
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<JwtTokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AbysaltoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError().OrResult(r => r.StatusCode == HttpStatusCode.TooManyRequests).WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
