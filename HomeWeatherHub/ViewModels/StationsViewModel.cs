using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Themes.Fluent;
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

namespace HomeWeatherHub.ViewModels;

public partial class StationsViewModel : ObservableObject
{
    
    public StationsViewModel() 
    {
        if (!Design.IsDesignMode)
        {

        }
    }

    [RelayCommand]
    public void ChangeStation(string id)
    {
        int stationID = 1;
        int.TryParse(id, out stationID);
        GlobalSettings.Settings.StationID = stationID;
        GlobalSettings.SettingsHelper.SaveSettings();
    }

}
