using Codebreak.AuthService.RPC;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.AuthService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Codebreak : AuthService";
            Console.WindowWidth += 50;

            XmlConfigurator.Configure();

            Auth.Manager.AuthManager.Instance.Initialize();

            Console.ReadLine();
        }
    }
}
