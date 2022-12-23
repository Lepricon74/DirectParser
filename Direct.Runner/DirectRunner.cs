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
using Direct.Shared.Logger;
using System;
using Direct.ImageRecognitionClient;
using Direct.ImageRecognitionClient.Helpers;
using Direct.ImageRecognitionClient.Interfaces;
using Direct.ImageRecognitionClient.Providers;
using Direct.ImageRecognitionClient.Services;

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
                    .AddSingleton<IUriProvider, DirectApiUrlProvider>(sp => 
                        new DirectApiUrlProvider(
                            new Uri(sp.GetService<IConfiguration>()["DirectSetting:DIRECT_API_URI"])))
                    .AddSingleton<IImageRecognitionUriProvider, ImageRecognitionApiUrlProvider>(sp =>
                        new ImageRecognitionApiUrlProvider(
                            new Uri(sp.GetService<IConfiguration>()["ImageRecognitionSetting:IMAGE_RECOGNITION_API_URI"])))
                    .AddSingleton<HttpClient>()
                    .AddSingleton<SafeJsonResponseDeserializer>()
                    .AddSingleton<DirectHttpRequestBuilder>()
                    .AddSingleton<DirectRequestSender>()
                    .AddSingleton<CampaignsService>()
                    .AddSingleton<AdGroupsService>()
                    .AddSingleton<AdsService>()
                    .AddSingleton<AdImagesService>()
                    .AddSingleton<DirectClient>()
                    .AddSingleton<ImageRecognitionRequestSender>()
                    .AddSingleton<ImageRecognitionService>()
                    .AddSingleton<ImageToTextRecognitionClient>()
                    .AddDirectParserContexWithConnectionString()
                    .AddSingleton<Func<IAdsRepository>>(sp => () => {
                        var dbContex = sp.GetService<DirectParserContex>();
                        var log = sp.GetService<ILog>();
                        return new SQLAdsRepository(dbContex, log);
                    })
                    .AddSingleton<Func<IAdImagesRepository>>(sp => () => {
                        var dbContex = sp.GetService<DirectParserContex>();
                        var log = sp.GetService<ILog>();
                        return new SQLAdImagesRepository(dbContex, log);
                    })
                    .AddSingleton<DirectParser>()
                    .AddHostedService(sp => 
                        new DirectParserService(
                                sp.GetService<DirectParser>(),
                                sp.GetService<ILog>(),
                                sp.GetService<Func<IAdsRepository>>(),
                                sp.GetService<Func<IAdImagesRepository>>(),
                                new TimeSpan(
                                    days : Int32.Parse((sp.GetService<IConfiguration>()["ParserCycle:Days"])),
                                    hours: Int32.Parse((sp.GetService<IConfiguration>()["ParserCycle:Hours"])),
                                    minutes: Int32.Parse((sp.GetService<IConfiguration>()["ParserCycle:Minutes"])),
                                    0
                                    )
                            )));
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
            var localLogger = new LocalLogger();
            var herculesLogger = new HerculesElkLogger(
                    localLogger,
                    configuration["HerculesSettings:apiKey"],
                    new HerculesGateClusterProvider(
                            new Uri(configuration["HerculesSettings:herculesGateUri"])
                        ),
                    configuration["HerculesSettings:environment"],
                    configuration["HerculesSettings:elkIndex"],
                    configuration["HerculesSettings:project"]
                );
            return services.AddSingleton<ILog>(_ => new CompositeLog(localLogger, herculesLogger));
        }
    }
}
