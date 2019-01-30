using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IpLocation.Models
{
    public interface IDataBaseProvider
    {
        Entity GetEntity(/*string*/IPAddress ip);

        void InsertLocation(Entity en);

        void InsertCity(Entity en);

        void InsertCountry(Entity en);

        void InsertContinent(Entity en);

        void InsertEntity(Entity en);

        void UpdateLocation(Entity en);

        void UpdateCity(Entity en);

        void UpdateEntity(Entity en);

    }
}
