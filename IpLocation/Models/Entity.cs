using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace IpLocation.Models
{
    public class Entity
    {
        //public Location Location { get; set; }

        public int? AccurasyRadius { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }



        //public City City { get; set; }

        public string CityName { get; set; }

        public int? CityId { get; set; }



        //public Country Country { get; set; }

        public string CountryName { get; set; }

        public int? CountryId { get; set; }

        public string CountryIsoCode { get; set; }



        //public Continent Continent { get; set; }

        public int? ContinentId { get; set; }

        public string ContinentCode { get; set; }

        public string ContinentName { get; set; }



        public IPAddress Ip { get; set; }
    }
}