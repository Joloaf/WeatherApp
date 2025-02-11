using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    internal class WeatherData
    {
        public int Id { get; set; }
        public string? Year { get; set; }
        public string? Month { get; set; }
        public string? Day { get; set; }
        public string? Time { get; set; }
        public string? Location { get; set; }
        public string? Temp { get; set; }
        public string? Humidity { get; set; }
    }
}
