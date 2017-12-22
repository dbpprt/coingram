using CoinGram.Common;
using CoinGram.Common.Coinigy;
using InfluxDB.LineProtocol.Client;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace CoinGram
{
    class Program
    {
        private readonly ILogger<Program> _logger;
        private readonly Application _application;
        private readonly AppSettings _appSettings;

        public Program(ILogger<Program> logger, IOptions<AppSettings> appSettings, Application application)
        {
            _logger = logger;
            _application = application;
            _appSettings = appSettings.Value;
        }

        private async Task RunAsync()
        {
            _logger.LogInformation($"coingram is starting with version {_appSettings.Version}");

            await _application.InitializeAsync();
            await _application.RunAsync(CancellationToken.None);
        }

        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            await serviceProvider.GetService<Program>().RunAsync();

            Console.ReadLine();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
               .AddJsonFile("appsettings.private.json", optional: true, reloadOnChange: false)
               .Build();

            var appSettings = configuration.GetSection("Configuration").Get<AppSettings>();
            
            serviceCollection
                .AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace).AddConsole().AddDebug())
                .AddOptions()
                .Configure<AppSettings>(configuration.GetSection("Configuration"))
                .AddSingleton<Program, Program>()
                .AddTransient<Application, Application>()
                .AddSingleton(new CoinigyApiClient(appSettings.CoinigyApiKey, appSettings.CoinigyApiSecret))
                .AddSingleton<ITelegramBotClient>(new TelegramBotClient(appSettings.TelegramApiKey))
                .AddSingleton(new LineProtocolClient(new Uri(appSettings.InfluxDbHost), appSettings.InfluxDbDatabase, appSettings.InfluxDbUser, appSettings.InfluxDbPassword))
                .AddMediatR(Assembly.GetExecutingAssembly());
               
        }
    }
}
