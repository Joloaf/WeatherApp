using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    internal class TextToDb
    {
        public static void InsertData()
        {
            string pattern = "(?<year>\\d{4})-(?<month>(0[1-9]|1[0-2]))-(?<day>(0[1-9]|[12][0-9]|3[01]))\\s(?<time>\\d{2}:\\d{2}:\\d{2}),(?<plats>\\w+),(?<temp>-?(4[0-9]|50|\\d{1,2})\\.\\d+),(?<luftfuktighet>(0|[1-9][0-9]?|100))\r\n";

            string[] lines = File.ReadAllLines(@"..\..\..\Files\tempData_medFel.txt");

        }
    }
}
