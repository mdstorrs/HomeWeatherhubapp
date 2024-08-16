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

namespace HomeWeatherHub.ViewModels;

public partial class MainViewModel : ObservableObject
{

    public MainViewModel()
    {
        if (!Design.IsDesignMode)
        {
            Task.Run(async () => await GetCurrent());
            RefreshTimer.Elapsed += RefreshTimer_Elapsed;
            RefreshTimer.Start();
        }
    }

    private System.Timers.Timer RefreshTimer = new System.Timers.Timer(10000);

    [ObservableProperty]
    public static CurrentReport _CurrentReport = new CurrentReport();

    [ObservableProperty]
    public static string _TempColor = "#888888";

    [ObservableProperty]
    public static string _LastUpdate = ""; //Online (Updated 20 seconds ago)

    private void RefreshTimer_Elapsed(object? sender, EventArgs e)
    {
        Task.Run(async () => await GetCurrent());
    }

    [RelayCommand]
    public async Task GetCurrent()
    {
        // Create an instance of HttpClient
        using (HttpClient client = new HttpClient())
        {
            // Set the base address for the HTTP client (optional)
            client.BaseAddress = new Uri("https://api.homeweatherhub.com");

            try
            {
                // Send a GET request to the specified endpoint
                HttpResponseMessage response = await client.GetAsync("/Current/1/1/");

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseData = await response.Content.ReadAsStringAsync();

                CurrentReport? report = Newtonsoft.Json.JsonConvert.DeserializeObject<CurrentReport>(responseData);

                if (report == null) {
                    return;
                }

                if (report.Success == false)
                {
                    return;
                }

                CurrentReport = report;

                decimal tempOut = 0;

                if (!decimal.TryParse(report.TempOutside, out tempOut)) { return; };

                if (tempOut <= 18)
                {
                    TempColor = "RoyalBlue";
                }
                else if (tempOut > 32)
                {
                    TempColor = "DarkRed";
                }
                else if (tempOut > 26)
                {
                    TempColor = "Chocolate";
                }
                else
                {
                    TempColor = "DarkGreen";
                }

                string lastUpdatedLabel = "";

                int totalSeconds = (int)(DateTime.Now - report.LastUpdated).TotalSeconds;

                lastUpdatedLabel = $"Online (Updated {totalSeconds.ToString()} seconds ago)";

                LastUpdate = lastUpdatedLabel;

            }
            catch (HttpRequestException e)
            {
                // Handle any HTTP request exceptions
                Console.WriteLine($"Request error: {e.Message}");
            }
        }
    }

}
