using AbySalto.Mid.Application.Common;
using AbySalto.Mid.WebApi.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace AbySalto.Mid
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("FE", policy =>
                {
                    policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            services.AddControllers();
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AbySalto", Version = "v1" });

                var jwtSS = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Enter token",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSS.Reference.Id, jwtSS);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSS, Array.Empty<string>() } });
            });

            services.AddProblemDetails();
            services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<JwtSettings>()
                .Bind(configuration.GetSection("Jwt"))
                .ValidateDataAnnotations()
                .Validate(o => !string.IsNullOrWhiteSpace(o.Key), "JWT Key is required")
                .ValidateOnStart();

            var provider = services.BuildServiceProvider();
            var jwtSettings = provider.GetRequiredService<IOptions<JwtSettings>>().Value;
            var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    //ClockSkew = TimeSpan.Zero
                };
            });

            return services;

        }
    }
}
