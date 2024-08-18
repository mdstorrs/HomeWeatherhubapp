using HomeWeatherHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWeatherHub.Business
{
    public static class GlobalSettings
    {
        public static SettingsHelper SettingsHelper = new SettingsHelper();
        public static WSSettings Settings = new WSSettings();

    }
}
