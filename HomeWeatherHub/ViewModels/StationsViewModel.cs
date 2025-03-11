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

public partial class StationsViewModel : ObservableObject
{
    [ObservableProperty]
    private List<Station> _Stations = new List<Station>();
    //public ObservableCollection<Station> Stations
    //{
    //    get => _stations;
    //    set => SetProperty(ref _stations, value);
    //}

    [ObservableProperty]
    private static bool _IsRefreshing;

    //public AsyncRelayCommand LoadDataCommand { get; }

    public StationsViewModel() 
    {
        //Stations = new ObservableCollection<Station>(); // Initialize
        //LoadDataCommand = new AsyncRelayCommand(LoadStations); // Initialize Command
        if (!Design.IsDesignMode)
        {
            //LoadStations();
        }
    }

    //[RelayCommand]
    //public void ChangeStation(string id)
    //{
    //    int stationID = 1;
    //    int.TryParse(id, out stationID);
    //    GlobalSettings.Settings.StationID = stationID;
    //    GlobalSettings.SettingsHelper.SaveSettings();
    //}

    [RelayCommand]
    private void ChangeStation(Station station)
    {
        if (station != null)
        {
            GlobalSettings.Settings.StationID = station.Id;
            GlobalSettings.SettingsHelper.SaveSettings();
        }
    }

    //[RelayCommand]
    //public void ChangeStation(Station station)
    //{
    //    //int stationID = 1;
    //    //int.TryParse(station.Id, out stationID);
    //    GlobalSettings.Settings.StationID = station.Id;
    //    GlobalSettings.SettingsHelper.SaveSettings();
    //}

    public async Task LoadStations()
    {
        // Create an instance of HttpClient
        using (HttpClient client = new HttpClient())
        {
            // Set the base address for the HTTP client (optional)
            client.BaseAddress = new Uri("https://api.homeweatherhub.com");

            try
            {

                IsRefreshing = true;

                Stations.Clear();

                int wsUnit = (int)GlobalSettings.Settings.PreferredUnit;

                // Send a GET request to the specified endpoint
                HttpResponseMessage response = await client.GetAsync($"/Stations/%20/0/0/");

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseData = await response.Content.ReadAsStringAsync();

                StationList? stationList = Newtonsoft.Json.JsonConvert.DeserializeObject<StationList>(responseData);

                if (stationList == null)
                {
                    return;
                }

                if (stationList.Success == false)
                {
                    return;
                }

                //List<Station> stations = new List<Station>();

                foreach (Station station in stationList.Stations)
                {
                    Stations.Add(station);
                }

                //Stations = stationList.Stations;          

            }
            catch (HttpRequestException e)
            {
                // Handle any HTTP request exceptions
                Console.WriteLine($"Request error: {e.Message}");
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
