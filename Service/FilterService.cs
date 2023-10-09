using AgroCoordenadas.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AgroCoordenadas.Service
{
    internal class FilterService : IFilter
    {
        public class FilteredData
        {
            public List<string>? N { get; set; }
            public List<string>? E { get; set; }
            public List<string>? Latitude { get; set; }
            public List<string>? Longitude { get; set; }
        }

        public FilteredData Filter(string text)
        {
            string pattern1 = @"(?<=\s+N\s+|\(\s*N\s*|\s*N=\s*)([\d,.]+)";
            string pattern2 = @"(?<=\s+E\s+|\(\s*E\s*|\s*E=\s*)([\d,.]+)";
            string pattern3 = @"(?<=\s*latitude\s*:\s*|\s*latitude\s*|\s*Lat\s*=\s*|\s*Latitude:\s*|\s*Latitude\s*de\s*|\s*Latitude\s*|\s*Lat\s*|\s*Lat:\s*|\(Latitude:\s*|\(Latitude\s*|\(Lat\s*|\(Lat:\s*)([-+°º'”\s\d,.]+)(""|”)";
            string pattern4 = @"(?<=\s*longitude\s*:\s*|\s*longitude\s*|\s*Long\s*=\s*|\s*Longitude:\s*|\s*Longitude\s*de\s*|\s*Longitude\s*|\s*Long\s*|\s*Long:\s*|\(Longitude:\s*|\(Longitude\s*|\(Long\s*|\(Long:\s*)([-+°º'""”\s\d,.]+)(""|”)";

            HashSet<string> seenValues1 = new();
            HashSet<string> seenValues2 = new();
            HashSet<string> seenValues3 = new();
            HashSet<string> seenValues4 = new();

            List<string> filteredNumbers1 = new();
            List<string> filteredNumbers2 = new();
            List<string> filteredNumbers3 = new();
            List<string> filteredNumbers4 = new();


            MatchCollection matches1 = Regex.Matches(text, pattern1);
            MatchCollection matches2 = Regex.Matches(text, pattern2);
            MatchCollection matches3 = Regex.Matches(text, pattern3);
            MatchCollection matches4 = Regex.Matches(text, pattern4);


            foreach (Match match in matches1)
            {
                string formattedValue = match.Value;
                if (seenValues1.Add(formattedValue))
                {
                    filteredNumbers1.Add(formattedValue);
                }

            }



            foreach (Match match in matches2)
            {
                string formattedValue = match.Value;
                if (seenValues2.Add(formattedValue))
                {
                    filteredNumbers2.Add(formattedValue);
                }
            }



            foreach (Match match in matches3)
            {
                string formattedValue = match.Value;
                if (seenValues3.Add(formattedValue))
                {
                    filteredNumbers3.Add(formattedValue);
                }
            }



            foreach (Match match in matches4)
            {
                string formattedValue = match.Value;
                if (seenValues4.Add(formattedValue))
                {
                    filteredNumbers4.Add(formattedValue);
                }
            }

            FilteredData results = new FilteredData
            {
                N = filteredNumbers1,
                E = filteredNumbers2,
                Latitude = filteredNumbers3,
                Longitude = filteredNumbers4
            };

            return results;
        }

    }
}
