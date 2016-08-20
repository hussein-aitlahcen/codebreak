using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Codebreak.Tool.Convert
{
    /// <summary>
    /// 
    /// </summary>
    public static class Program
    {
        private static string INPUT_MAP_FOLDER = "./maps/input/";
        private static string OUTPUT_MAP_FOLDER = "./maps/output/";
        private static string OUTPUT_MAP_FILE = OUTPUT_MAP_FOLDER + "result.sql";
        private static string REAL_MAP_QUERY = "insert into maptemplate values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');";
        private static string REGEX_MAP_QUERY = @"VALUES \('(?<Id>[^']*)', '(?<CreateTime>[^']*)', '(?<Width>[^']*)', '(?<Height>[^']*)', '(?<Data>[^']*)', '(?<DataKey>[^']*)', '(?<Places>[^']*)', '(?<Monsters>[^']*)', '(?<Capabilities>[^']*)', '(?<Position>[^']*)',";

        private static string INPUT_TRIGGER_FOLDER = "./trigger/input/";
        private static string OUTPUT_TRIGGER_FOLDER = "./trigger/output/";
        private static string OUTPUT_TRIGGER_FILE = OUTPUT_TRIGGER_FOLDER + "result.sql";
        private static string REAL_TRIGGER_QUERY = "insert into maptrigger values ('{0}', '{1}', '', '2005:mapId={2},cellId={3}');";
        private static string REGEX_TRIGGER_QUERY = @"VALUES\((?<MapId>[0-9]*), (?<CellId>[0-9]*).*'(?<NextMapId>[0-9]*),(?<NextCellId>[0-9]*)'";

        private static string INPUT_JSON_FOLDER = "./json/input/";
        private static string OUTPUT_JSON_FOLDER = "./json/output/";
        private static string OUTPUT_JSON_FILE = OUTPUT_JSON_FOLDER + "result.sql";
        private static string REAL_JSON_QUERY = "insert into monstersuperrace values ('{0}', '{1}');";
        private static string REGEX_JSON_QUERY = @".*\[(?<RaceId>[0-9]*)].*""(?<Name>.*)"".*""(?<SuperRaceId>[0-9]*)""";

        private static string INPUT_MONSTER_FOLDER = "./monsters/input/";
        private static string OUTPUT_MONSTER_FOLDER = "./monsters/output/";
        private static string OUTPUT_MONSTER_FILE = OUTPUT_MONSTER_FOLDER + "result.sql";
        private static string REAL_MONSTER_QUERY = "insert into monsterspawn select 3, {0}, (select id from monstergrade where monsterid = {1} and level = {2} limit 1);";
        private static string REGEX_MONSTER_QUERY = @"'(?<MapId>[0-9]*)'.*'\|(?<Monsters>[^']*)";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ProcessMaps();
            ProcessTriggers();
            ProcessJsons();
            ProcessMonstersSpawn();

            Console.Read();
        }
        
        /// <summary>
        /// 
        /// </summary>
        private static void ProcessMonstersSpawn()
        {
            Console.WriteLine("Loading monsters ...");

            if (!Directory.Exists(INPUT_MONSTER_FOLDER))
                Directory.CreateDirectory(INPUT_MONSTER_FOLDER);

            if (!Directory.Exists(OUTPUT_MONSTER_FOLDER))
                Directory.CreateDirectory(OUTPUT_MONSTER_FOLDER);

            Regex expression = new Regex(REGEX_MONSTER_QUERY, RegexOptions.None);
            var inputFiles = Directory.GetFiles(INPUT_MONSTER_FOLDER, "*.sql");

            Console.WriteLine(inputFiles.Length + " file(s) loaded.");

            StringBuilder outputBuilder = new StringBuilder();

            foreach (var file in inputFiles)
            {
                foreach (var line in File.ReadAllLines(file))
                {
                    foreach (Match match in expression.Matches(line))
                    {
                        if (match.Success)
                        {
                            Console.WriteLine("Processing monster ...");

                            var monsters = match.Groups["Monsters"].Value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var monster in monsters)
                            {
                                var monsterId = monster.Split(',')[0];
                                var monsterLevel = monster.Split(',')[1];
                                outputBuilder.AppendLine(string.Format
                                (
                                    REAL_MONSTER_QUERY,
                                    match.Groups["MapId"],
                                    monsterId,
                                    monsterLevel
                                ));
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Saving monsters ...");

            File.WriteAllText(OUTPUT_MONSTER_FILE, outputBuilder.ToString());

            Console.WriteLine("Monsters done.");
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ProcessJsons()
        {
            Console.WriteLine("Loading triggers ...");

            if (!Directory.Exists(INPUT_JSON_FOLDER))
                Directory.CreateDirectory(INPUT_JSON_FOLDER);

            if (!Directory.Exists(OUTPUT_JSON_FOLDER))
                Directory.CreateDirectory(OUTPUT_JSON_FOLDER);

            Regex expression = new Regex(REGEX_JSON_QUERY, RegexOptions.None);
            var inputFiles = Directory.GetFiles(INPUT_JSON_FOLDER, "*.txt");

            Console.WriteLine(inputFiles.Length + " file(s) loaded.");

            StringBuilder outputBuilder = new StringBuilder();

            foreach (var file in inputFiles)
            {
                foreach (var line in File.ReadAllLines(file))
                {
                    foreach (Match match in expression.Matches(line))
                    {
                        if (match.Success)
                        {
                            Console.WriteLine("Processing json ...");

                            outputBuilder.AppendLine(string.Format
                            (
                                REAL_JSON_QUERY,
                                match.Groups["RaceId"],
                                match.Groups["Name"],
                                match.Groups["SuperRaceId"]
                            ));
                        }
                    }
                }
            }

            Console.WriteLine("Saving jsons ...");

            File.WriteAllText(OUTPUT_JSON_FILE, outputBuilder.ToString());

            Console.WriteLine("Jsons done.");
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ProcessTriggers()
        {
            Console.WriteLine("Loading triggers ...");

            if (!Directory.Exists(INPUT_TRIGGER_FOLDER))
                Directory.CreateDirectory(INPUT_TRIGGER_FOLDER);

            if (!Directory.Exists(OUTPUT_TRIGGER_FOLDER))
                Directory.CreateDirectory(OUTPUT_TRIGGER_FOLDER);

            Regex expression = new Regex(REGEX_TRIGGER_QUERY, RegexOptions.None);
            var inputFiles = Directory.GetFiles(INPUT_TRIGGER_FOLDER, "*.sql");

            Console.WriteLine(inputFiles.Length + " file(s) loaded.");

            StringBuilder outputBuilder = new StringBuilder();

            foreach (var file in inputFiles)
            {
                foreach (var line in File.ReadAllLines(file))
                {
                    foreach (Match match in expression.Matches(line))
                    {
                        if (match.Success)
                        {
                            Console.WriteLine("Processing trigger ...");
                            
                            outputBuilder.AppendLine(string.Format
                            (
                                REAL_TRIGGER_QUERY,
                                match.Groups["MapId"],
                                match.Groups["CellId"],
                                match.Groups["NextMapId"],
                                match.Groups["NextCellId"]
                            ));
                        }
                    }
                }
            }

            Console.WriteLine("Saving triggers ...");

            File.WriteAllText(OUTPUT_TRIGGER_FILE, outputBuilder.ToString());

            Console.WriteLine("Triggers done.");
        }

        /// <summary>
        /// 
        /// </summary>
        private static void ProcessMaps()
        {
            Console.WriteLine("Loading maps ...");

            if (!Directory.Exists(INPUT_MAP_FOLDER))
                Directory.CreateDirectory(INPUT_MAP_FOLDER);

            if (!Directory.Exists(OUTPUT_MAP_FOLDER))
                Directory.CreateDirectory(OUTPUT_MAP_FOLDER);

            Regex expression = new Regex(REGEX_MAP_QUERY, RegexOptions.None);
            var inputFiles = Directory.GetFiles(INPUT_MAP_FOLDER, "*.sql");

            Console.WriteLine(inputFiles.Length + " file(s) loaded.");

            StringBuilder outputBuilder = new StringBuilder();

            foreach (var file in inputFiles)
            {
                foreach (var line in File.ReadAllLines(file))
                {
                    foreach (Match match in expression.Matches(line))
                    {
                        if (match.Success)
                        {
                            Console.WriteLine("Processing map ...");

                            var posData = match.Groups["Position"].Value.Split(',');
                            var x = posData[0];
                            var y = posData[1];
                            var subAreaId = posData[2];

                            outputBuilder.AppendLine(string.Format
                            (
                                REAL_MAP_QUERY,
                                match.Groups["Id"],
                                match.Groups["CreateTime"],
                                match.Groups["Data"],
                                match.Groups["DataKey"],
                                match.Groups["Places"],
                                match.Groups["Width"],
                                match.Groups["Height"],
                                x,
                                y,
                                match.Groups["Capabilities"],
                                subAreaId
                            ));
                        }
                    }
                }
            }

            Console.WriteLine("Saving maps ...");

            File.WriteAllText(OUTPUT_MAP_FILE, outputBuilder.ToString());

            Console.WriteLine("Maps done.");
        }
    }
}
