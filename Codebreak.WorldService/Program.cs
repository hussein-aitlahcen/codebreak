using Codebreak.Framework.Generic;
using Codebreak.WorldService.RPC;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Codebreak : WorldService";
            Console.WindowWidth += 50;

            XmlConfigurator.Configure();

            World.Manager.WorldManager.Instance.Initialize();

            Console.ReadLine();
        }
    }
}
