using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWeatherHub.Models
{
    public class BaseReport : ResponseData
    {

        public int WSID { get; set; }
        public string PassKey { get; set; } = "";
        public string WSName { get; set; } = "My Home";
        public ReportType Type { get; set; }
        public MeasurementSystem Measurement { get; set; }
        public string MeasurementSymbol { get; set; } = "C";

        public enum ReportType
        {
            Current,
            Day,
            Week,
            Month,
            Year,
            All
        }

        public enum MeasurementSystem
        {
            Imperial,
            Metric
        }

    }
}
