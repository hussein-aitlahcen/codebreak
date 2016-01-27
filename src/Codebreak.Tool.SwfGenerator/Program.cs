using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Tool.SwfGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Directory.Exists("output"))
                Directory.CreateDirectory("output");

            if (!Directory.Exists("input"))
                Directory.CreateDirectory("input");

            File.WriteAllText(
                "output/versions_fr.txt", 
                string.Concat(
                    "&f=",
                    string.Join(
                        "|", 
                        Directory.GetFiles(
                            "input", 
                            "*.swf"
                        ).Select(
                            path => Path.GetFileNameWithoutExtension(path).Replace("_", ",")
                        )
                    )
                )
            );
        }
    }
}
