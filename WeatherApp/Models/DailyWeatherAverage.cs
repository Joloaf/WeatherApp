using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class DailyWeatherAverage
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public double AverageTemp { get; set; }
        public double AverageHumidity { get; set; }
    }
}
