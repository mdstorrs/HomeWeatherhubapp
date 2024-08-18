using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Platform;
using Avalonia.Themes.Fluent;
using HomeWeatherHub.ViewModels;
using HomeWeatherHub.Views;
using System;

namespace HomeWeatherHub;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        SetTheme();
        SubscribeToThemeChanges();

        base.OnFrameworkInitializationCompleted();
    }

    private void SubscribeToThemeChanges()
    {
        var platformSettings = Avalonia.Application.Current.PlatformSettings;
        platformSettings.ColorValuesChanged += OnPlatformColorValuesChanged;
    }

    private void OnPlatformColorValuesChanged(object? sender, PlatformColorValues e)
    {
        // Update the theme when the system theme changes
        SetTheme();
    }

    private void SetTheme()
    {
        var platformSettings = Avalonia.Application.Current.PlatformSettings;
        var isDarkMode = platformSettings?.GetColorValues().ThemeVariant == PlatformThemeVariant.Dark;

        if (isDarkMode)
        {
            // Load Dark Theme
            this.Styles[0] = new StyleInclude(new Uri("avares://HomeWeatherHub/"))
            {
                Source = new Uri("avares://HomeWeatherHub/Styles/Dark.axaml")
            };
        }
        else
        {
            // Load Light Theme
            this.Styles[0] = new StyleInclude(new Uri("avares://HomeWeatherHub/"))
            {
                Source = new Uri("avares://HomeWeatherHub/Styles/Light.axaml")
            };
        }
    }

}
