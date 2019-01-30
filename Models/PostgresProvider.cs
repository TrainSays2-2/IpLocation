using DataModels;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace IpLocation.Models
{
    public class PostgresProvider : IDataBaseProvider
    {
        public Entity GetEntity(/*string*/IPAddress ip)
        {
            try
            {
                Entity location = new Entity();

                using (var conn = new LocationsBaseDB())
                {
                    var loc =
                            from locat in conn.Locations
                            join city in conn.Cities on locat.Id equals city.GeoNameId
                            join count in conn.Countries on city.CountryNameId equals count.GeoNameId
                            join conti in conn.Continents on count.ContinentNameId equals conti.GeoNameId
                            where locat.Ip.Equals(ip)
                            select new
                            {
                                _Ip = locat.Ip,
                                _Latitude = locat.Latitude,
                                _Longitude = locat.Longitude,
                                _AccurasyRadius = locat.AccurasyRadius,
                                _CityName = city.Name,
                                _CityGeoNameId = city.GeoNameId,
                                _CountryName = count.Name,
                                _CountryIsoCode = count.IsoCode,
                                _CountryGeoNameId = count.GeoNameId,
                                _ContinentName = conti.Name,
                                _ContinentCode = conti.Code,
                                _ContinentGeoNameId = conti.GeoNameId
                            };

                    foreach (var l in loc)
                    {
                        location.Location.AccurasyRadius = l._AccurasyRadius;
                        location.Location.Latitude = l._Latitude;
                        location.Location.Longitude = l._Longitude;

                        location.City.Name = l._CityName.ToString();
                        location.City.GeoNameId = l._CityGeoNameId;

                        location.Country.Name = l._CountryName.ToString();
                        location.Country.IsoCode = l._CountryIsoCode.ToString();
                        location.Country.GeoNameId = l._CountryGeoNameId;

                        location.Continent.Name = l._ContinentName.ToString();
                        location.Continent.Code = l._ContinentCode.ToString();
                        location.Continent.GeoNameId = l._ContinentGeoNameId;
                        location.Ip = /*IPAddress.Parse(ip)*/ip;
                    }
                }

                return location;//location;
            }
            catch (Exception ex)
            {
                //nlog
                return null;
            }
        }

        public void InsertLocation(Entity en)
        {

            try
            {
                using (var conn = new LocationsBaseDB())
                {
                    conn.Locations
                        .Value(p => p.Id, en.City.GeoNameId)
                        .Value(p => p.Ip, en.Ip)
                        .Value(p => p.Latitude, en.Location.Latitude)
                        .Value(p => p.Longitude, en.Location.Longitude)
                        .Insert();
                }

            }
            catch (Exception ex)
            {
            }
        }

        public void InsertCity(Entity en)
        {
            try
            {
                using (var conn = new LocationsBaseDB())
                {
                    conn.Cities
                        .Value(p => p.CountryNameId, en.Country.GeoNameId)
                        .Value(p => p.GeoNameId, en.City.GeoNameId)
                        .Value(p => p.Name, en.City.Name)
                        .Insert();
                }
            }
            catch (Exception ex)
            {
            }

        }

        public void InsertCountry(Entity en)
        {
            try
            {
                using (var conn = new LocationsBaseDB())
                {
                    conn.Countries
                        .Value(p => p.ContinentNameId, en.Continent.GeoNameId)
                        .Value(p => p.GeoNameId, en.Country.GeoNameId)
                        .Value(p => p.Name, en.Country.Name)
                        .Value(p => p.IsoCode, en.Country.IsoCode)
                        .Insert();
                }
            }
            catch (Exception ex)
            {
            }

        }

        public void InsertContinent(Entity en)
        {
            try
            {
                using (var conn = new LocationsBaseDB())
                {
                    conn.Continents
                        .Value(p => p.GeoNameId, en.Continent.GeoNameId)
                        .Value(p => p.Name, en.Continent.Name)
                        .Value(p => p.Code, en.Continent.Code)
                        .Insert();
                }
            }
            catch (Exception ex)
            {
            }

        }

        public void InsertEntity(Entity en)
        {
            try
            {
                InsertContinent(en);
                InsertCountry(en);
                InsertCity(en);
                InsertLocation(en);
            }
            catch (Exception ex)
            {
            }

        }

        public void UpdateCity(Entity en)
        {
            try
            {
                using (var conn = new LocationsBaseDB())
                {
                    conn.Cities
                        .Where(
                            p => p.CountryNameId != en.Country.GeoNameId &&
                            p.Name == en.City.Name
                            ) 
                            .Set(p => p.GeoNameId, en.City.GeoNameId)
                            .Set(p => p.Name, en.City.Name)
                        .Update();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void UpdateLocation(Entity en)
        {
            using (var conn = new LocationsBaseDB())
            {
                conn.Locations
                    .Where(
                        p => p.Id != en.City.GeoNameId &&
                        p.Ip == en.Ip
                        )
                        .Set(p => p.Id, en.City.GeoNameId)
                        .Set(p => p.Latitude, en.Location.Latitude)
                        .Set(p => p.Longitude, en.Location.Longitude)
                    //.Set(p => p.Ip, en.Ip)
                    .Update();
            }
        }

        public void UpdateEntity(Entity en)
        {

        }

    }
}