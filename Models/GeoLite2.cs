using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using IpLocation.Models;
using MaxMind.GeoIP2;
namespace IpLocation.Models
{


    public class GeoLite2 : IBackgroundBaseUpdater
    {
        private const long  MAXIP = 0x00000000FFFFFFFF;

        private IDataBaseProvider _db;

        public GeoLite2()
        {
            _db = new PostgresProvider();
        }

        public GeoLite2(IDataBaseProvider db)
        {
            _db = db;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public async void UpdateAsync()
        {
            try
            {
                IPAddress ip = new IPAddress(0);

                using (var client = new WebServiceClient(0, "000000000000"))
                {
                    var en = new Entity();

                    for (long i = 0; i < MAXIP; ++i)
                    {
                        ip.Address = i;

                        en = await SetEntity(client, ip/*.Address.ToString()*/);
                        if (_db?.GetEntity(ip) == null)
                        {
                            _db.InsertEntity(en);
                        }
                        else
                        {
                            _db.UpdateEntity(en);
                        }
                        //_db.InsertEntity(en): 
                        //    _db.UpdateEntity(en);
                        ////_db.UpdateEntity(en);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task<Entity> SetEntity(/*ref*/ WebServiceClient cl, /*string*/IPAddress ip)
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

            en.Ip = /*IPAddress.Parse(ip)*/ip;

            return en;
        }
    }
}