using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Codebreak.Tool.Convert
{
    class Program
    {
        static string INPUT_FOLDER = "./input/";
        static string OUTPUT_FOLDER = "./output/";
        static string OUTPUT_FILE = OUTPUT_FOLDER + "result.sql";
        static string REAL_QUERY = "insert into maptemplate values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}');";
        static string REGEX_QUERY = @"VALUES \('(?<Id>[^']*)', '(?<CreateTime>[^']*)', '(?<Width>[^']*)', '(?<Height>[^']*)', '(?<Data>[^']*)', '(?<DataKey>[^']*)', '(?<Places>[^']*)', '(?<Monsters>[^']*)', '(?<Capabilities>[^']*)', '(?<Position>[^']*)',";
        
        static void Main(string[] args)
        {
            Console.WriteLine("Loading...");

            if (!Directory.Exists(INPUT_FOLDER))
                Directory.CreateDirectory(INPUT_FOLDER);

            if(!Directory.Exists(OUTPUT_FOLDER))
                Directory.CreateDirectory(OUTPUT_FOLDER);

            Regex expression = new Regex(REGEX_QUERY, RegexOptions.None);
            var inputFiles = Directory.GetFiles(INPUT_FOLDER, "*.sql");

            Console.WriteLine(inputFiles.Length + " file(s) loaded.");

            StringBuilder outputBuilder = new StringBuilder();

            foreach(var file in inputFiles)
            {
                foreach (var line in File.ReadAllLines(file))
                {
                    foreach (Match match in expression.Matches(line))
                    {
                        if (match.Success)
                        {
                            Console.WriteLine("Processing...");

                            var posData = match.Groups["Position"].Value.Split(',');
                            var x = posData[0];
                            var y = posData[1];
                            var subAreaId = posData[2];

                            outputBuilder.AppendLine(string.Format
                                (
                                    REAL_QUERY,
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

            Console.WriteLine("Saving...");

            File.WriteAllText(OUTPUT_FILE, outputBuilder.ToString());

            Console.WriteLine("Done.");

            Console.Read();
        }
    }
}
