using DataModels;
using LinqToDB;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace IpLocation.Models
{
    public class PostgresProvider : IDataBaseProvider
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Entity GetEntity(/*string*/IPAddress ip)
        {                    

            try
            {

                using (var conn = new LocationsBaseDB())
                {

                    Entity location = new Entity();

                    var loc =
                            from locat in conn.Locations
                            //join city in conn.Cities on locat.Id equals city.GeoNameId
                            //join count in conn.Countries on city.CountryNameId equals count.GeoNameId
                            //join conti in conn.Continents on count.ContinentNameId equals conti.GeoNameId
                            where locat.Ip.Address == ip
                            select new 
                            {
                                _Ip = locat.Ip,
                                _Latitude = locat.Latitude,
                                _Longitude = locat.Longitude,
                                _AccurasyRadius = locat.AccurasyRadius,
                                _CityName = locat.CityName ?? " ",//city.Name,
                                _CityId = locat.CityId,//city.GeoNameId,
                                _CountryName = locat.CountryName ?? " ",//count.Name,
                                _CountryIsoCode = locat.CountryIsoCode ?? " ",//count.IsoCode,
                                _CountryId = locat.CountryId,//count.GeoNameId,
                                _ContinentName = locat.ContinentName ?? " ",//conti.Name,
                                _ContinentCode = locat.ContinentCode ?? " ",//conti.Code,
                                _ContinentId = locat.ContinentId,//conti.GeoNameId
                            };

                    foreach (var l in loc.ToList())
                    {
                        var en = new Entity()
                        {
                            CityId = l._CityId,
                            CityName = l._CityName,

                            CountryName = l._CountryName,
                            CountryIsoCode = l._CountryIsoCode,
                            CountryId = l._CountryId,

                            ContinentName = l._ContinentName,
                            ContinentCode = l._ContinentCode,
                            ContinentId = l._ContinentId,

                            AccurasyRadius = l._AccurasyRadius,
                            Latitude = l._Latitude,
                            Longitude = l._Longitude,

                            Ip = l._Ip.Address
                        };

                        location = en;
                    }

                    return location;
                    //location.Ip == null ? null : location;//location;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Get_Entity");

                return null;
            }
        }

        public void InsertEntity(Entity en)
        {
            try
            {
                using (var conn = new LocationsBaseDB())
                {
                    conn.Locations
                        //.Value(p => p.Id, en.City.GeoNameId)
                        .Value(p => p.ContinentName, en.ContinentName)
                        .Value(p => p.ContinentCode, en.ContinentCode)
                        .Value(p => p.ContinentId, en.ContinentId)

                        .Value(p => p.CountryName, en.CountryName)
                        .Value(p => p.CountryId, en.CountryId)
                        .Value(p => p.CountryIsoCode, en.CountryIsoCode)
                            
                        .Value(p => p.CityName, en.CityName)
                        .Value(p => p.CityId, en.CityId)

                        .Value(p => p.Ip, en.Ip)
                        .Value(p => p.Latitude, en.Latitude)
                        .Value(p => p.Longitude, en.Longitude)
                    .Insert();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Insert_Entity");
            }

        }

        public void UpdateEntity(Entity en)
        {
            try
            {
                using (var conn = new LocationsBaseDB())
                {
                    conn.Locations
                        //.Value(p => p.Id, en.City.GeoNameId)
                        .Where(p => p.Ip == en.Ip)
                        .Set(p => p.ContinentName, en.ContinentName)
                        .Set(p => p.ContinentCode, en.ContinentCode)
                        .Set(p => p.ContinentId, en.ContinentId)

                        .Set(p => p.CountryName, en.CountryName)
                        .Set(p => p.CountryId, en.CountryId)
                        .Set(p => p.CountryIsoCode, en.CountryIsoCode)

                        .Set(p => p.CityName, en.CityName)
                        .Set(p => p.CityId, en.CityId)

                        .Set(p => p.Latitude, en.Latitude)
                        .Set(p => p.Longitude, en.Longitude)
                    .Update();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Update_Entity");
            }
        }

    }
}
