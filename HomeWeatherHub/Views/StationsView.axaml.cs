using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using HomeWeatherHub.ViewModels;
using System;

namespace HomeWeatherHub.Views;

public partial class StationsView : UserControl
{
    public StationsView()
    {
        InitializeComponent();
        this.AttachedToVisualTree += MainView_AttachedToVisualTree;
    }

    private async void MainView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (DataContext is StationsViewModel vm)
        {
            await vm.LoadStations(); // Ensure stations are loaded
            lstStations.Items.Clear();
            foreach (var st in vm.Stations)
            {
                lstStations.Items.Add(st);
            }
        }
    }

}
