using HomeWeatherHub.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime;
using System.Text.Json;

namespace HomeWeatherHub.Business;

public class SettingsHelper
{

    public event EventHandler? SettingsChangedEvent;

    // Method to trigger the event
    protected virtual void OnSettingsChanged()
    {
        SettingsChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    private  readonly string SettingsFile = "settings.json";

    public  void LoadSettings()
    {
        try
        {

            if (File.Exists(SettingsFile))
            {
                string? json = File.ReadAllText(SettingsFile);
                if (json != null)
                {
                    GlobalSettings.Settings = JsonSerializer.Deserialize<WSSettings>(json);
                }
            }

            if (GlobalSettings.Settings == null)
                GlobalSettings.Settings = new WSSettings();

            OnSettingsChanged();

        }
        catch (System.Exception)
        {

            throw;
        }

    }

    public  void SaveSettings()
    {
        try
        {
            //File.WriteAllText(SettingsFile, JsonSerializer.Serialize(GlobalSettings.Settings));
            OnSettingsChanged();
        }
        catch (System.Exception)
        {

            throw;
        }
    }
}