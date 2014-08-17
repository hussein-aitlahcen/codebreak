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
            ConfigureConsole();
            XmlConfigurator.Configure();

            Logger.Info("Starting the auth service...");

            InitializeGCServer();

            AuthService.Instance.Start("./Config.json");

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
            System.Console.WindowWidth += 40;
        }
    }
}
