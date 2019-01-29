using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpLocation.Models
{
    public interface IBackgroundBaseUpdater
    {
        void Update(string basePath);
    }
}
