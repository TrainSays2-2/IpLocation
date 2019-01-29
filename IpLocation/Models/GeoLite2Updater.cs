using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using IpLocation.Models;
using MaxMind.GeoIP2;
using NLog;

namespace IpLocation.Models
{


    public class GeoLite2Updater : IBackgroundBaseUpdater
    {
        private const long  MAX_IP = 0x00000000FFFFFFFF;

        private static Logger logger = LogManager.GetCurrentClassLogger();


        private IDataBaseProvider _db;

        public GeoLite2Updater()
        {
            _db = new PostgresProvider();
        }

        public GeoLite2Updater(IDataBaseProvider db)
        {
            _db = db;
        }

        public void Update(string basePath)
        {
            try
            {
                IPAddress ip = new IPAddress(0);

                //using (var client = new WebServiceClient(999999, "000000000000"))
                using (var reader = new DatabaseReader(basePath))
                {

                    for (long i = 0; i < MAX_IP; ++i)
                    {
                        ip.Address = i;

                        //en = await SetEntity(client, ip/*.Address.ToString()*/);

                        try
                        {
                            //var resp = reader.City(ip.ToString());

                            var en = CreateEntity(reader, ip);

                            if (_db.GetEntity(ip).Ip != null)
                            {
                                 _db.UpdateEntity(en);
                            }
                            else
                            {
                                _db.InsertEntity(en);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex, "GeoLite2Updater");

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "GeoLite2Updater");
            }
        }

        private Entity CreateEntity(DatabaseReader reader, IPAddress ip)
        {
            var resp = reader.City(ip.ToString());

            var en = new Entity()
            {
                AccurasyRadius = resp.Location.AccuracyRadius,
                Latitude = resp.Location.Latitude,
                Longitude = resp.Location.Longitude,

                CityName = resp.City.Name,
                CityId = resp.City.GeoNameId,

                CountryName = resp.Country.Name,
                CountryId = resp.Country.GeoNameId,
                CountryIsoCode = resp.Country.IsoCode,

                ContinentCode = resp.Continent.Code,
                ContinentId = resp.Continent.GeoNameId,
                ContinentName = resp.Continent.Name,

                Ip = ip
            };

            return en;
        }

    }
}
