using Crc32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Tool.UplauncherPatchGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach(var file in Directory.GetFiles("./input"))
            {
                Console.WriteLine("#########");
                Console.WriteLine("#########");
                var infos = new FileInfo(file);
                Crc32Algorithm crc32 = new Crc32Algorithm();
                String hash = String.Empty;

                using (FileStream fs = File.Open(file, FileMode.Open))
                    foreach (byte b in crc32.ComputeHash(fs)) hash += b.ToString("x2").ToLower();

                Console.WriteLine("file=" + infos.Name + " crc=" + hash);
            }

            Console.ReadLine();
        }
    }
}
