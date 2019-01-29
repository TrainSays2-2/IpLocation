using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using IpLocation.Models;
using MaxMind.GeoIP2;

namespace IpLocation.App_Start
{
    public class GeoLite2 : IBackgroundBaseUpdater
    {
        public void Update()
        {
            throw new NotImplementedException();
        }

        public async void UpdateAsync()
        {
            //MaxMind.GeoIP2.DatabaseReader()
            using (var client = new WebServiceClient(0, "000000000000"))
            {
                var en = new Entity();
                //en = await SetEntity(client, );
            }
        }

        private async Task<Entity> SetEntity(/*ref*/ WebServiceClient cl, string ip)
        {
            var en = new Entity();
            var response = await cl.CityAsync(ip);

            en.Location.AccurasyRadius = response.Location.AccuracyRadius;
            en.Location.Latitude = response.Location.Latitude;
            en.Location.Longitude = response.Location.Longitude;

            en.City.Name = response.City.Name;
            en.City.GeoNameId = response.City.GeoNameId;

            en.Country.Name = response.Country.Name;
            en.Country.IsoCode = response.Country.IsoCode;
            en.Country.GeoNameId = response.Country.GeoNameId;

            en.Continent.Name = response.Continent.Name;
            en.Continent.Code = response.Continent.Code;
            en.Continent.GeoNameId = response.Continent.GeoNameId;

            en.Ip = IPAddress.Parse(ip);

            return en;
        }
    }
}