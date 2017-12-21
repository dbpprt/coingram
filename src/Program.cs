using CoinGram.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace CoinGram
{
    class Program
    {
        private readonly ILogger<Program> _logger;
        private readonly AppSettings _appSettings;

        public Program(ILogger<Program> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        private void Run()
        {
            _logger.LogInformation($"coingram is starting with version {_appSettings.Version}...");
            _logger.LogError(_appSettings.TelegramApiKey);
        }

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<Program>().Run();

            Console.ReadLine();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
               .AddJsonFile("appsettings.private.json", optional: true, reloadOnChange: false)
               .Build();

            serviceCollection
                .AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace).AddConsole().AddDebug())
                .AddOptions()
                .Configure<AppSettings>(configuration.GetSection("Configuration"))
                .AddSingleton<Program, Program>();
        }
    }
}
