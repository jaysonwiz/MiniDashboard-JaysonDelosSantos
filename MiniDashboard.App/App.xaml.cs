using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniDashboard.App.Services;
using MiniDashboard.App.ViewModels;
using MiniDashboard.App.Views;
using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Windows;

namespace MiniDashboard.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
              .ConfigureServices((context, services) =>
              {
                  // Register dialog service as singleton
                  services.AddSingleton<IDialogService, DialogService>();

                  // Register components / pages
                  services.AddTransient<ProductAddEditViewModel>();
                  services.AddTransient<ProductAddEditView>();
                  services.AddTransient<ProductViewModel>();
                  services.AddTransient<ProductView>();
                  services.AddTransient<DashboardViewModel>();
                  services.AddTransient<DashboardView>();                                  

                  // Register main window
                  services.AddSingleton<MainViewModel>();
                  services.AddSingleton<MainWindow>();

                 

                  // Register HttpClient manually
                  services.AddSingleton<HttpClient>(sp =>
                  {
                      var handler = new HttpClientHandler
                      {
                          // For dev only: bypass SSL validation if needed
                          ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                      };

                      var client = new HttpClient(handler)
                      {
                          BaseAddress = new Uri("http://localhost:5125") // your API base
                      };

                      return client;
                  });



              })
              .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();


            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            AppHost.Dispose();
            base.OnExit(e);
        }
    }

}
