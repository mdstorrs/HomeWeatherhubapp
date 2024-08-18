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
using HomeWeatherHub.Views;
using HomeWeatherHub.Business;
using System.Collections.ObjectModel;
using System.Data;
using System.ComponentModel.Design;

namespace HomeWeatherHub.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        if (!Design.IsDesignMode)
        {
            GlobalSettings.SettingsHelper.SettingsChangedEvent += OnSettingsChanged;
            GlobalSettings.SettingsHelper.LoadSettings();

        }
    }

    [ObservableProperty]
    public bool _HasStationID = false;

    [ObservableProperty]
    public UserControl? _CurrentControl;

    public StationsView? stationsView { get; set; }
    public CurrentView? currentView { get; set; }
    public HistoryView? historyView { get; set; }

    // Event handler that will be called when the event is fired
    private void OnSettingsChanged(object? sender, EventArgs e)
    {
        if (GlobalSettings.Settings.StationID == -1)
            ShowStations();
        else
            ShowCurrent();
    }

    [RelayCommand]
    public void ShowStations()
    {
        if (stationsView == null)
        {
            stationsView = new StationsView()
            {
                DataContext = new StationsViewModel()
            };
        }
        CurrentControl = stationsView;
        HasStationID = (GlobalSettings.Settings.StationID != -1);
    }

    [RelayCommand]
    public void ShowCurrent()
    {
        if (currentView==null)
        {
            currentView = new CurrentView()
            {
                DataContext = new CurrentViewModel()
            };
        }
        CurrentControl = currentView;
        HasStationID = (GlobalSettings.Settings.StationID != -1);
    }

    [RelayCommand]
    public void ShowHistory()
    {
        if (historyView == null)
        {
            historyView = new HistoryView()
            {
                DataContext = new HistoryViewModel()
            };
        }
        CurrentControl = historyView;
        HasStationID = (GlobalSettings.Settings.StationID != -1);
    }

    [RelayCommand]
    public void ShowSettings()
    {

    }

}
