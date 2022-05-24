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
using System.Threading;
using Direct.Parser.Database.Interfaces;
using Direct.Parser.Database.Repositories;
using Direct.Parser.Database;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.Configuration;
using Direct.Parser.Database.Providers;
using Direct.Parser;
using Direct.Parser.Database;
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
                    .AddSingleton<ILog, DirectParserLogger>()
                    .AddSingleton<IAuthTokenProvider>(_ => new AuthTokenProvider(Constants.AUTH_TOKEN))
                    .AddSingleton<IUriProvider, DirectApiSandboxUrlProvider>()
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
                        configuration.GetConnectionString("PostgreSQLLocalConnection"), 
                        b => b.MigrationsAssembly("Direct.Runner")));
        }
    }
}
