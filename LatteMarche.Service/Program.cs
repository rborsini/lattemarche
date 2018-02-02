using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topshelf;

namespace LatteMarche.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<ServiceStartUp>(s =>
                {
                    s.ConstructUsing(name => new ServiceStartUp());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("LatteMarche.Service");
                x.SetDisplayName("LatteMarche.Service");
                x.SetServiceName("LatteMarche.Service");
            });
        }
    }
}
