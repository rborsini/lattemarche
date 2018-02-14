using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatteMarche.SynchJob
{
    class Program
    {
        static void Main()
        {
            AutoFacConfig.Configure();
            JobHostConfiguration config = new JobHostConfiguration();
            config.UseTimers();
            JobHost host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
