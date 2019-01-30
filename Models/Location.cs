using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IpLocation.Models
{
    public class Location
    {
        public int? AccurasyRadius { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

    }
}