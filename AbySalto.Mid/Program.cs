
using AbySalto.Mid.Application;
using AbySalto.Mid.Infrastructure;
using AbySalto.Mid.Infrastructure.Persistence;
using AbySalto.Mid.WebApi.Extensions;

namespace AbySalto.Mid
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddPresentation()
                .AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddJwtAuthentication(builder.Configuration);

            var app = builder.Build();

            app.VerifyMigrations<AbysaltoDbContext>();

            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Desk Link");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseCors("FE");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
