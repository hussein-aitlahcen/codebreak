using System.Runtime;
using Codebreak.Service.Auth;
using log4net;
using log4net.Config;

namespace Codebreak.App.AuthConsole
{
    /// <summary>
    /// 
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            ConfigureConsole();

            Logger.Info("Starting auth service...");

            InitializeGCServer();

            AuthService.Instance.Start("./config.json");

            while(true)
                System.Console.ReadLine();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void InitializeGCServer()
        {
            GCSettings.LatencyMode = GCSettings.IsServerGC ? GCLatencyMode.Batch : GCLatencyMode.Interactive;
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ConfigureConsole()
        {
            System.Console.WindowWidth = 100;
            System.Console.Title = "Codebreak : AuthService";
            Logger.Info("   ###################################");
            Logger.Info("  #####################################");
            Logger.Info(" ####           CODEBREAK           ####");
            Logger.Info("#####         SERVICE.AUTH          #####");
            Logger.Info(" ####            Smarken            ####");
            Logger.Info("  #####################################");
            Logger.Info("   ###################################");
        }
    }
}
