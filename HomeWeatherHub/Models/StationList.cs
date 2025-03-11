using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWeatherHub.Models
{
    public class StationList : ResponseData
    {
        public List<Station> Stations { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public StationList() { Stations = new List<Station>(); }
    }
}
