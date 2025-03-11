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
using Avalonia.Data;

namespace HomeWeatherHub.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    
    public HistoryViewModel() 
    {
        SelectedOption = (int)dateMode.day;
        if (!Design.IsDesignMode)
        {
            Task.Run(async () => await GetReport());
        }
    }

    private int CurrentID;
    private int AddDate = 0;
    private DateTime FromDate = DateTime.Now.Date;
    private DateTime ToDate = DateTime.Now.Date.AddDays(1);

    private enum dateMode : int
    {
        day,
        week,
        month,
        year,
        all
    }

    [ObservableProperty]
    public static HistoryReport _HistoryReport = new HistoryReport();

    [ObservableProperty]
    public static string _DateRangeLabelText = "Today";

    [ObservableProperty]
    public static bool _NextEnabled = false;

    [ObservableProperty]
    private int _SelectedOption;

    [RelayCommand]
    public async Task Back() 
    {
        AddDate += 1;
        await GetReport();
    }

    [RelayCommand]
    public async Task Next()
    {
        AddDate -= 1;
        await GetReport();
    }

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

                GetDateRange();

                //Report. Day, Week, Month, year etc
                int rep = 1;

                //Measurement System
                BaseReport.MeasurementSystem ms = BaseReport.MeasurementSystem.Metric;
                int msInt = (int)ms;

                DateTime date = this.FromDate;
                string dateString = date.ToString("yyyy-MM-dd");

                // Send a GET request to the specified endpoint
                HttpResponseMessage response = await client.GetAsync($"/History/{GlobalSettings.Settings.StationID}/{rep}/{dateString}/{msInt}");

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

    private void GetDateRange()
    {

        int addDate = this.AddDate; //The number of days back from this date.
        dateMode mode = (dateMode)SelectedOption; // dateMode.day; //Day, Week, Month, Year, All

        //mode = int.Parse(this.ddlRange.SelectedValue);
        //if (int.TryParse(Master.Request.QueryString["add"], out addDate)) { this.AddDate = addDate; } else { this.AddDate = 0; }

        switch (mode)
        {
            case dateMode.week:
                DateTime day = this.ToDate = DateTime.Now.Date.AddDays(-(addDate * 7) + 1);
                this.FromDate = GetWSWeek(day);
                this.ToDate = this.FromDate.AddDays(7);
                DateRangeLabelText = $"{this.FromDate.ToShortDateString()} to {this.ToDate.AddDays(-1).ToShortDateString()}";
                break;
            case dateMode.month:
                DateTime thisMonthStart = DateTime.Now.Date.AddDays(-(DateTime.Now.Day - 1));
                this.FromDate = thisMonthStart.AddMonths(-(addDate));
                this.ToDate = this.FromDate.AddMonths(1);
                DateTimeFormatInfo dtfi = CultureInfo.GetCultureInfo("en-US").DateTimeFormat;
                DateRangeLabelText = $"{dtfi.GetMonthName(this.FromDate.Month)} {this.FromDate.Year}";
                break;
            case dateMode.year:
                this.ToDate = new DateTime(DateTime.Now.Date.AddYears(-(addDate) + 1).Date.Year, 1, 1);
                this.FromDate = this.ToDate.AddYears(-1);
                DateRangeLabelText = $"{this.FromDate.Year}";
                break;
            case dateMode.all:
                this.FromDate = new DateTime(2000, 1, 1);
                this.ToDate = DateTime.Now.Date.AddDays(1);
                DateRangeLabelText = "All Time";
                break;
            default:
                this.ToDate = DateTime.Now.Date.AddDays(-(addDate) + 1);
                this.FromDate = this.ToDate.AddDays(-1);
                if (addDate == 0)
                {
                    DateRangeLabelText = "Today";
                }
                else
                {
                    DateRangeLabelText = $"{this.FromDate.ToShortDateString()}";
                }
                break;
        }

        //Unhide this to see the proper date range
        //this.lblDateRange.Text = $"{this.FromDate.ToShortDateString()} to {this.ToDate.AddDays(-1).ToShortDateString()}";

        //Set the post back urls for the next and previous buttons
        this.SetButtonPostBackUrls();

        //Onlye enable the next button when required
        if (this.AddDate == 0) { NextEnabled = false; } else { NextEnabled = true; }

    }

    private void SetButtonPostBackUrls()
    {

        //btnPrevious.PostBackUrl = $"History.aspx?id={this.CurrentID}&add={this.AddDate + 1}&mode={int.Parse(this.ddlRange.SelectedValue)}";
        //btnNext.PostBackUrl = $"History.aspx?id={this.CurrentID}&add={this.AddDate - 1}&mode={int.Parse(this.ddlRange.SelectedValue)}";

    }

    public static DateTime GetWSWeek(DateTime someDate)
    {
        int dow;
        if ((int)someDate.DayOfWeek == 0)
        {
            dow = 7;
        }
        else
        {
            dow = (int)someDate.DayOfWeek;
        }

        DateTime fromDate = someDate.AddDays(-(dow - 1)).Date;

        return fromDate;

    }


}
