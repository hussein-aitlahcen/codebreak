using System.Runtime;
using log4net;
using log4net.Config;

namespace Codebreak.App.WorldConsole
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            ConfigureConsole();

            Logger.Info("Starting world service...");

            InitializeGCServer();
            Service.World.WorldService.Instance.Start("./Config.json");

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
            System.Console.Title = "Codebreak : WorldService";
            Logger.Info("   ###################################");
            Logger.Info(" #######################################");
            Logger.Info("#####           CODEBREAK           #####");
            Logger.Info("#####             WORLD             #####");
            Logger.Info(" #######################################");
            Logger.Info("   ###################################");
        }
    }
}
