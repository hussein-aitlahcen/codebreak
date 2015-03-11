using Crc32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Codebreak.Tool.UplauncherPatchGenerator
{
    public class Program
    {
        public static List<file> modifications = new List<file>();

        static void Main(string[] args)
        {
            Console.Write("Version courante : ");
            var currentVersion = Console.ReadLine();

            Console.Write("Version suivante : ");
            var nextVersion = Console.ReadLine();

            var patchFileName = "dofus_" + currentVersion + "_to_" + nextVersion + ".zip";


            using (var stream = new FileStream("./input/games.xml", FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(GamesConfigurations));
                serializer.Serialize(stream, new GamesConfigurations() { dofus = new dofus() { version = nextVersion } });
            }

            var fullPath = new Uri(Path.GetFullPath("./input/"));
            var files = new List<file>();
            foreach (var file in Directory.GetFiles("./input", "*.*", SearchOption.AllDirectories))
            {
                Console.WriteLine("#########");
                var infos = new FileInfo(file);
                Crc32Algorithm crc32 = new Crc32Algorithm();
                StringBuilder hash = new StringBuilder();

                using (FileStream fs = File.Open(file, FileMode.Open))
                    foreach (byte b in crc32.ComputeHash(fs))
                        hash.Append(b.ToString("x2").ToLower());

                var fileUri = new Uri(infos.FullName);

                files.Add(new file()
                {
                    nameTo = fullPath.MakeRelativeUri(fileUri).ToString(),
                    nameFrom = infos.Name,
                    action = "add",
                    crc = hash.ToString().ToUpper()
                });
            }

            using (var stream = new FileStream("./output/manifest.xml", FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(Modifications));
                serializer.Serialize(stream, new Modifications() { files = files.ToArray() });
            }

            if (File.Exists("./output/" + patchFileName))
                File.Delete("./output/" + patchFileName);
            using (ZipArchive patchFile = ZipFile.Open("./output/" + patchFileName, ZipArchiveMode.Create))
            {
                patchFile.CreateEntryFromFile("./output/manifest.xml", "manifest.xml");
                foreach (var file in Directory.GetFiles("./input", "*.*", SearchOption.AllDirectories))
                {
                    var infos = new FileInfo(file);
                    var fileUri = new Uri(infos.FullName);
                    var relative = fullPath.MakeRelativeUri(fileUri).ToString();
                    patchFile.CreateEntryFromFile(file, relative);
                }
            }

            {
                Crc32Algorithm crc32 = new Crc32Algorithm();
                StringBuilder hash = new StringBuilder();
                using (FileStream fs = File.Open("./output/" + patchFileName, FileMode.Open))
                    foreach (byte b in crc32.ComputeHash(fs))
                        hash.Append(b.ToString("x2").ToLower());
                
                using (var stream = new FileStream("./output/dofus_" + currentVersion + ".xml", FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(Uplauncher));
                    serializer.Serialize(stream, new Uplauncher() { version = nextVersion, patchs = new patch[] { new patch() { path = new path() { crc = hash.ToString().ToUpper(), url = "http://staticdata.earthscape.fr/client/" + patchFileName } } } });
                }
            }

            Console.ReadLine();
        }

        [Serializable]
        public class GamesConfigurations
        {
            public dofus dofus;            
        }
        
        [Serializable]
        public class dofus
        {
            public string path = ".";
            public string version = "";
        }

        [XmlRoot("UpLauncher")]
        [XmlType("UpLauncher")]
        [Serializable]
        public class Uplauncher
        {
            [XmlAttribute("version")]
            public string updater_version = "1.8";
            [XmlAttribute]
            public string path = "http://staticdata.earthscape.fr/updater/___________/";
            [XmlAttribute]
            public string win = "uplauncher_dofus_0.72.exe";

            public string version = "1.29.2";

            [XmlElement("file")]
            public patch[] patchs;
        }
        
        [Serializable]
        public class patch
        {
            [XmlAttribute]
            public string type = "patch";

            [XmlElement("path")]
            public path path;
        }

        [Serializable]
        public class path
        {
            [XmlAttribute]
            public string crc;
            [XmlText]
            public string url;
        }

        [XmlType("modifications")]
        [Serializable]
        public class Modifications
        {
            [XmlElement("file")]
            public file[] files;
        }
        
        [Serializable]
        public class file
        {
            [XmlAttribute]
            public string nameTo;
            [XmlAttribute]
            public string action;
            [XmlAttribute]
            public string nameFrom;
            [XmlAttribute]
            public string crc;
        }
    }
}
