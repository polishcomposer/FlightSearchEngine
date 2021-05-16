using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSE.Models
{
    public class UserFlight
    {
        [Key]
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Price { get; set; }
        public string TotalFlightTimes { get; set; }
        public string TotalDates { get; set; }
        public string BookingLink { get; set; }
        public string TotalTime { get; set; }
        public string FlightPlaces { get; set; }
        public string Details { get; set; }
    }
}
