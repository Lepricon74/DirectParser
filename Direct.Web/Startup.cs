using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using Direct.Parser.Database;
using Vostok.Logging.Abstractions;
using Direct.Shared.Logger;

namespace Direct.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //string connection = Configuration.GetConnectionString("PostgreSQLLocalConnection");
            string connection = Configuration.GetConnectionString("PostgreSQLExternalConnection");
            services.AddDbContext<DirectParserContex>(options => options.UseNpgsql(connection));
            AddLogger(services);
            services.AddCors();
            services.AddControllersWithViews();
        }
        private void AddLogger(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var localLogger = new LocalLogger();
            var herculesLogger = new HerculesElkLogger(
                localLogger,
                Configuration["HerculesSettings:apiKey"],
                new HerculesGateClusterProvider(
                    new Uri(Configuration["HerculesSettings:herculesGateUri"])
                ),
                Configuration["HerculesSettings:environment"],
                Configuration["HerculesSettings:elkIndex"],
                Configuration["HerculesSettings:project"]
            );
            //services.AddSingleton<ILog>(_ => new CompositeLog(localLogger, herculesLogger));
            services.AddSingleton<ILog>(_ => localLogger);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                endpoints.MapControllers();
            });
        }
    }
}
