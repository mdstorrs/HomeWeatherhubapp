using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
//using Avalonia.Themes.Fluent;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using System.IO;
using System.Net;
using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using HomeWeatherHub.Models;
using Avalonia.Controls;
using System.Diagnostics.Metrics;
using System.Globalization;
using HomeWeatherHub.Business;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.ComponentModel;

namespace HomeWeatherHub.ViewModels;

public partial class SettingsViewModel : ObservableObject
{

    [ObservableProperty]
    private static bool _IsRefreshing;

    public SettingsViewModel() 
    {
        if (!Design.IsDesignMode)
        {

        }
    }

}
