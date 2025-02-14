using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class WeatherData
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public double Temp { get; set; }
        public double Humidity { get; set; }
        public double MoldRisk { get; set; }
    }
}
