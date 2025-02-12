using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class SaveToFile
    {
        public static string path = "../../../Files/";

        public static void SaveMonthlyAverages(List<dynamic> monthlyAverage)
        {
            string fileName = "WeatherData.txt";
            using (StreamWriter streamWriter = new StreamWriter(path + fileName, false))
            {
                streamWriter.WriteLine("Utomhus:");
                foreach (var item in monthlyAverage.Where(m => m.Location.ToLower() == "ute"))
                {
                    streamWriter.WriteLine($"Datum: {item.Year}-{item.Month} | Medeltemp: {item.AverageTemp:F1}°C, Medelfuktighet: {item.AverageHumidity:F1}%");
                }

                streamWriter.WriteLine();

                streamWriter.WriteLine("Inomhus:");
                foreach (var item in monthlyAverage.Where(m => m.Location.ToLower() == "inne"))
                {
                    streamWriter.WriteLine($"Datum: {item.Year}-{item.Month} | Medeltemp: {item.AverageTemp:F1}°C, Medelfuktighet: {item.AverageHumidity:F1}%");
                }
            }
        }
    }
}