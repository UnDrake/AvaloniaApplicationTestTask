using Avalonia;
using AvaloniaApplicationTestTask.Services;
using AvaloniaApplicationTestTask.Services.Interfaces;
using AvaloniaApplicationTestTask.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AvaloniaApplicationTestTask
{
    internal class Program
    {
        private static ServiceProvider _serviceProvider;

        [STAThread]
        public static void Main(string[] args)
        {
            _serviceProvider = ConfigureServices();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IDatabaseService, DatabaseService>();
            services.AddSingleton<IGraphGenerator, GraphGenerator>();
            services.AddTransient<MainWindow>();

            return services.BuildServiceProvider();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
