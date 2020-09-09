using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;

namespace MiniProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitializeLogAndConfiguration();
            CreateHostBuilder(args).Build().Run();
        }

        private static IConfiguration Configuration { get; set; }

        private static void InitializeLogAndConfiguration()
        {
            Log.Logger = SetupSerilog(new LoggerConfiguration());
            Configuration = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                       .Build();
        }

        private static Serilog.ILogger SetupSerilog(LoggerConfiguration config)
        {
            return config
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs\\app-{Date}.log"),
                        outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                        rollingInterval: RollingInterval.Day)
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
