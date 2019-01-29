using IpLocation.Models;
using IpLocation.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IpLocation.Controllers
{
    public class IpController : ApiController
    {
        private IConcreteLocationRepository _locationRepository;

        public IpController()
        {
            _locationRepository = new ConcreteLocationRepository(db: new PostgresProvider());
        }

        public IpController(IConcreteLocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        // GET api/Ip/GetIp?ip={string}
        [HttpGet]
        public Entity GetIp(string ip)
        {
            return _locationRepository.GetConcreteLocation(ip);
            //return "value";
        }

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
