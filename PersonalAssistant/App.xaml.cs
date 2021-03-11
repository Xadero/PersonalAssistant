using Microsoft.Extensions.DependencyInjection;
using PersonalAssistant;
using PersonalAssistant.Service.Interfaces;
using PersonalAssistant.Service.Services;
using System;
using System.Windows;

namespace PersonalAssistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISoundService, SoundService>();
            services.AddScoped<ISpeechRecognizerService, SpeechRecognizerService>();
            services.AddScoped<IAssistantService, AssistantService>();
            services.AddSingleton<ClassicPersonalAssistant>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<ClassicPersonalAssistant>();
        }
    }
}
