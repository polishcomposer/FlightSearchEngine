using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSE.Models
{
    public class Coordinates
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class NameTranslations
    {
        public string en { get; set; }
    }

    public class Root
    {
        public string country_code { get; set; }
        public string code { get; set; }
        public Coordinates coordinates { get; set; }
        public string name { get; set; }
        public string time_zone { get; set; }
        public NameTranslations name_translations { get; set; }
        public object cases { get; set; }
    }
}
