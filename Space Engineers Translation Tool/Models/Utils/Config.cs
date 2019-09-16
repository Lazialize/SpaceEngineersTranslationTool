using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Newtonsoft.Json;
using Space_Engineers_Translation_Tool.Models.Objects;

namespace Space_Engineers_Translation_Tool.Models.Utils
{
    class ConfigData
    {
        [JsonProperty("SpaceEngineersDirectory")]
        public string SpaceEngineersDirectory { get; set; }

        [JsonProperty("IsItemTranslationEnabled")]
        public bool IsItemTranslationEnabled { get; set; }

        [JsonProperty("IsBlockTranslationEnabled")]
        public bool IsBlockTranslationEnabled { get; set; }
    }
    class Config
    {
        public const string ConfigFile = "config.json";
        public static ConfigData ConfigData { get; } =
            JsonConvert.DeserializeObject<ConfigData>(File.ReadAllText(ConfigFile));

        public static void Save()
        {
            using (var sw = new StreamWriter(ConfigFile, false, Encoding.UTF8))
            {
                sw.Write(JsonConvert.SerializeObject(ConfigData));
            }
        }
    }
}
