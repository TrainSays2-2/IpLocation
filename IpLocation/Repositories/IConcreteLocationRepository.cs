using IpLocation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpLocation.Repositories
{
    public interface IConcreteLocationRepository
    {
        Entity GetConcreteLocation(string ip);
        IEnumerable<Entity> GetLocations(string startIp, string endIp);
    }
}
