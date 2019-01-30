using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IpLocation.Models
{
    public class Continent
    {
        public string Name { get; set; }

        //public Dictionary<string, string> Names { get; set; }

        public int? GeoNameId { get; set; }

        public string Code { get; set; }
    }
}