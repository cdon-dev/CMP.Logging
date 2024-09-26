using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace SfWeb
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //NOTE : https://github.com/MicrosoftDocs/azure-docs/issues/27395#issuecomment-473767218
            services.AddApplicationInsightsTelemetry(_configuration);
            services.AddRouting();
            services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var coreLoggerFactory = context.RequestServices.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();

                    Log.Logger.Information("Serilog Logger - Hello World");
                    coreLoggerFactory.CreateLogger("Starup").LogInformation(".NET Core Logger - Hello World");
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}