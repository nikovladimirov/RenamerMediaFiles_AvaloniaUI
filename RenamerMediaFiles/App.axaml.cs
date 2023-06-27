using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using RenamerMediaFiles.Models;
using RenamerMediaFiles.Services.Implementations;
using RenamerMediaFiles.Services.Interfaces;
using RenamerMediaFiles.ViewModels;
using RenamerMediaFiles.Views;

namespace RenamerMediaFiles;

public partial class App : Application
{
    public App()
    {
        Services = ConfigureServices();
    }

    /// <summary>
    /// Gets the current <see cref="App"/> instance in use
    /// </summary>
    public new static App Current => (App)Application.Current;

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
    /// </summary>
    public IServiceProvider Services { get; }

    /// <summary>
    /// Configures the services for the application.
    /// </summary>
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IFileService, JsonFileService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<SettingsModel, SettingsModel>();
        services.AddSingleton<SettingsViewModel, SettingsViewModel>();
        services.AddSingleton<MainModel, MainModel>();
        services.AddSingleton<MainViewModel, MainViewModel>();

        return services.BuildServiceProvider();
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetService<MainViewModel>()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = Services.GetService<MainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}