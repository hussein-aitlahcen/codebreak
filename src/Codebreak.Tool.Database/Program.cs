using Codebreak.Framework.Configuration;
using Codebreak.Framework.Configuration.Providers;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Codebreak.Tool.Database
{
    static class Program
    {
        static string CONFIG_PATH = "./config.json";

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            XmlConfigurator.Configure();

            var configManager = new ConfigurationManager();
            configManager.RegisterAttributes(Assembly.GetAssembly(typeof(Program)));
            configManager.Add(new JsonConfigurationProvider(CONFIG_PATH), true);
            configManager.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
