using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpLocation.App_Start
{
    public interface IBackgroundBaseUpdater
    {
        void UpdateAsync();
        void Update();
    }
}
