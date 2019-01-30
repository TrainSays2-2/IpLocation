using DataModels;
using IpLocation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace IpLocation.Repositories
{
    public class ConcreteLocationRepository : IConcreteLocationRepository
    {

        private IDataBaseProvider _db;

        public ConcreteLocationRepository(IDataBaseProvider db)
        {
            _db = db;
        }

        public Entity GetConcreteLocation(string ip)
        {
            try
            {
                return _db.GetEntity(IPAddress.Parse(ip));//location;
            }
            catch (Exception ex)
            {
                //nlog
                return null;
            }

            //return location;
        }

        public IEnumerable<Entity> GetLocations(string startIp, string endIp)
        {
            throw new NotImplementedException();
        }
    }
}