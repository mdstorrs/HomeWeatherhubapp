using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWeatherHub.Models
{
    public class CurrentReport : BaseReport
    {
        public DateTime LastUpdated { get; set; }
        public string TempOutside { get; set; } = "...";
        public string TempInside { get; set; } = "...";
        public string HumidityOutside { get; set; } = "...";
        public string HumidityInside { get; set; } = "...";
        public string Pressure { get; set; } = "...";
        public int UVIndex { get; set; } = 0;
        public string RainRate { get; set; } = "...";
        public string RainAccumulation { get; set; } = "...";
        public int WindDirAngle { get; set; } = 0;
        public string WindDirection { get; set; } = "...";
        public string WindSpeed { get; set; } = "...";
        public string WindGust { get; set; } = "...";
        public string TempFeel { get; set; } = "...";

    }
}
