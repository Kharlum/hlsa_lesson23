using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Mime;

namespace WebApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services
                .AddHealthChecks()
                .AddCheck("api",
                    () => HealthCheckResult.Healthy(),
                   tags: new[] { "healthy" });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/api/health/healthy", new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains("healthy"),
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                    },
                    ResponseWriter = async (context, report) =>
                    {
                        context.Response.ContentType = MediaTypeNames.Application.Json;
                        var result = JsonConvert.SerializeObject(
                        new
                        {
                            checks = report.Entries.Select(entryReport =>
                            new
                            {
                                description = entryReport.Key,
                                status = entryReport.Value.Status.ToString(),
                                responseTime = entryReport.Value.Duration.TotalMilliseconds
                            }),
                            totalResponseTime = report.TotalDuration.TotalMilliseconds,
                            status = report.Status.ToString()
                        });
                        await context.Response.WriteAsync(result);
                    }
                });
                endpoints.MapControllers();
            });
        }
    }
}
