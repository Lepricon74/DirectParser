using System.Threading.Tasks;
using Direct.Client.Interfaces;
using Direct.Client.Providers;
using Direct.Client.Services;
using Direct.Client;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Direct.Client.Extensions;
using Direct.Client.Helpers;
using Vostok.Logging.Abstractions;
using Microsoft.Extensions.Hosting;
using Direct.Parser.Database.Interfaces;
using Direct.Parser.Database.Repositories;
using Direct.Parser.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Direct.Parser;
using Direct.Runner.Logger;
using System;

namespace Direct.Runner
{
    class DirectRunner
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                services
                    .AddLogger()
                    .AddSingleton<IAuthTokenProvider>(sp => 
                        new AuthTokenProvider(
                            sp.GetService<IConfiguration>()["DirectSetting:AUTH_TOKEN"]))
                    .AddSingleton<IUriProvider>(sp => 
                        new DirectApiUrlProvider(
                            new Uri(sp.GetService<IConfiguration>()["DirectSetting:DIRECT_API_URI"])))
                    .AddSingleton<HttpClient>()
                    .AddSingleton<SafeJsonResponseDeserializer>()
                    .AddSingleton<DirectHttpRequestBuilder>()
                    .AddSingleton<DirectRequestSender>()
                    .AddSingleton<CampaignsService>()
                    .AddSingleton<AdGroupsService>()
                    .AddSingleton<AdsService>()
                    .AddSingleton<DirectClient>()
                    .AddDirectParserContexWithConnectionString()
                    .AddSingleton<Func<IAdsRepository>>(sp => () => {
                        var dbContex = sp.GetService<DirectParserContex>();
                        var log = sp.GetService<ILog>();
                        return new SQLAdsRepository(dbContex, log);
                    })
                    .AddSingleton<DirectParser>()
                    .AddHostedService<DirectParserService>());
    }

    internal static class DependencyInjectionExtensions {
        internal static IServiceCollection AddDirectParserContexWithConnectionString(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            return services.AddDbContext<DirectParserContex>(
                    options => options.UseNpgsql(
                        //configuration.GetConnectionString("PostgreSQLLocalConnection"),
                        configuration.GetConnectionString("PostgreSQLExternalConnection"),
                        b => b.MigrationsAssembly("Direct.Runner")));
        }
        
        internal static IServiceCollection AddLogger(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            var localLogger = new DirectRunnerLocalLogger();
            var herculesLogger = new DirectRunnerHerculesElkLogger(
                    localLogger,
                    configuration["HerculesSettings:apiKey"],
                    new HerculesGateClusterProvider(
                            new Uri(configuration["HerculesSettings:herculesGateUri"])
                        ),
                    configuration["HerculesSettings:environment"],
                    configuration["HerculesSettings:elkIndex"]
                );
            return services.AddSingleton<ILog>(_ => new CompositeLog(localLogger, herculesLogger));
        }
    }
}
