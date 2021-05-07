using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FlightSE.Models
{
    public class IndexModel
    {
        public JObject MyData { get; set; }
        public JObject MyFlight { get; internal set; }
    }
}
