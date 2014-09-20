using System.Runtime;
using Codebreak.Service.Auth;
using log4net;
using log4net.Config;

namespace Codebreak.App.AuthConsole
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            ConfigureConsole();

            Logger.Info("Starting auth service...");

            InitializeGCServer();

            AuthService.Instance.Start("./Config.json");

            while(true)
                System.Console.ReadLine();
        }

        // ReSharper disable once InconsistentNaming
        private static void InitializeGCServer()
        {
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;
        }

        private static void ConfigureConsole()
        {
            System.Console.Title = "Codebreak : AuthService";
            Logger.Info("   ###################################");
            Logger.Info(" #######################################");
            Logger.Info("#####           CODEBREAK           #####");
            Logger.Info("#####             AUTH              #####");
            Logger.Info(" #######################################");
            Logger.Info("   ###################################");
        }
    }
}
