using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Utils.Configuration
{
    /// <summary>
    /// Handles .ini file data.
    /// </summary>
    public class Settings
    {
        private static Settings _settings;
        private string filePath;

        public List<INISection> Sections = new List<INISection>();

        public static Settings Instance
        {
            get
            {
                if (_settings == null)
                    _settings = new Settings();
                return _settings;
            }
        }

        public void Load(string path)
        {
            filePath = path;
            var currentSection = string.Empty;

            using (var sr = File.OpenText(path))
            {
                var line = string.Empty;
                while ((line = sr.ReadLine()) != null)
                {
                    if (currentSection != string.Empty)
                    {
                        if (line.Contains("="))
                        {
                            var keyValue = line.Split('=');
                            var section = Sections.Find(x => x.Name == currentSection);
                            try { section.AddKeyValue(keyValue[0], keyValue[1]); }
                            catch
                            {
                                section = new INISection() { Name = currentSection };
                                section.AddKeyValue(keyValue[0], keyValue[1]);
                                Sections.Add(section);
                            }
                        }
                        else if (line.StartsWith("["))
                            currentSection = string.Empty;
                    }

                    if (line.StartsWith("#")) continue;
                    if (line.StartsWith("["))
                        currentSection = line;
                }
            }
        }

        public void Save()
        {
            using (var sw = new StreamWriter(filePath))
            {
                foreach (var section in Sections)
                {
                    sw.WriteLine(section.Name);

                    var keyValues = section.GetKeyValues();
                    foreach (var pair in keyValues)
                    {
                        sw.WriteLine($"{pair.Key}={pair.Value.ToLower()}");
                    }
                }
            }
        }

        public INISection GetSection(string name)
        {
            name = "[" + name + "]";
            return Sections.Find(x => x.Name == name);
        }
    }
}
