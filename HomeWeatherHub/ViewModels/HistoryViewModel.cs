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

public partial class HistoryViewModel : ObservableObject
{
    
    public HistoryViewModel() 
    {
        if (!Design.IsDesignMode)
        {

        }
    }

    [ObservableProperty]
    public static HistoryReport _HistoryReport = new HistoryReport();

    [RelayCommand]
    public async Task GetReport()
    {
        // Create an instance of HttpClient
        using (HttpClient client = new HttpClient())
        {
            // Set the base address for the HTTP client (optional)
            client.BaseAddress = new Uri("https://api.homeweatherhub.com");

            try
            {
                // Send a GET request to the specified endpoint
                HttpResponseMessage response = await client.GetAsync("/History/1/1/1/1/1/");

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the response content as a string
                string responseData = await response.Content.ReadAsStringAsync();

                HistoryReport? report = Newtonsoft.Json.JsonConvert.DeserializeObject<HistoryReport>(responseData);

                if (report == null)
                {
                    return;
                }

                if (report.Success == false)
                {
                    return;
                }

                HistoryReport = report;

                //decimal tempOut = 0;

                //if (!decimal.TryParse(report.TempOutside, out tempOut)) { return; };

                //if (tempOut <= 18)
                //{
                //    TempColor = "RoyalBlue";
                //}
                //else if (tempOut > 32)
                //{
                //    TempColor = "DarkRed";
                //}
                //else if (tempOut > 26)
                //{
                //    TempColor = "Chocolate";
                //}
                //else
                //{
                //    TempColor = "DarkGreen";
                //}

            }
            catch (HttpRequestException e)
            {
                // Handle any HTTP request exceptions
                Console.WriteLine($"Request error: {e.Message}");
            }
        }
    }

}
