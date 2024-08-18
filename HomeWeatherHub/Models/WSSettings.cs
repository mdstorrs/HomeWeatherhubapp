using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWeatherHub.Models
{
    public class WSSettings
    {

        public WSSettings() { }

        public bool LoggedIn { get; set; } = false;
        public int UserID { get; set; } = -1;
        public string? Username { get; set; }
        public string? EmailAddress { get; set; }
        public eAccessLevel AccessLevel { get; set; }
        public eUnit PreferredUnit { get; set; } = eUnit.Celsius;
        public string? PassKey { get; set; }
        public int StationID { get; set; } = -1;
        public string? StationName { get; set; }
        public string? IPAddress { get; set; }

        public enum eAccessLevel
        {
            None = 0,
            User = 1,
            Host = 2,
            PowerHost = 5,
            Admin = 10,
            Blocked = 99
        }

        public enum eUnit
        {
            Fahrenheit = 0,
            Celsius = 1
        }
    }
}
