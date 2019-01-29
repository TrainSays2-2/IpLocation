using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace IpLocation.Models
{
    public class Entity
    {
        public Location Location { get; set; }

        public City City { get; set; }

        public Country Country { get; set; }

        public Continent Continent { get; set; }

        public IPAddress Ip { get; set; }
    }
}