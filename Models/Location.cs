using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightSE.Models
{
    public class Location
    {
        [Key]
        public int ID { get; set; }
        public string Code { get; set; }
        public string AirportLocation { get; set; }
    }
}
