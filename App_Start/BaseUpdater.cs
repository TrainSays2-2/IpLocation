using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using IpLocation.Models;

namespace IpLocation.App_Start
{
    public class BaseUpdater
    {
        private IBackgroundBaseUpdater _upd;

        public BaseUpdater(IBackgroundBaseUpdater upd)
        {
            _upd = upd;
            Thread th = new Thread(()=>Update());
        }

        void Update()
        {
            _upd.Update();
        }
    }
}