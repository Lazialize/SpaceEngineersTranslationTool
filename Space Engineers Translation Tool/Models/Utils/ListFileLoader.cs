using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Space_Engineers_Translation_Tool.Models.Utils
{
    class ListFileLoader
    {
        public static List<Dictionary<string, string>> Load(string filePath)
        {
            return JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(File.ReadAllText(filePath));
        }

        // ModのLocalization対応を想定して、複数のjsonファイルを読み込めるようにしている。
        public static List<Dictionary<string, string>> LoadAll()
        {
            var returnValue = new List<Dictionary<string, string>>();

            foreach (var fileName in Directory.GetFiles("ListFiles", "*.json"))
            {
                returnValue = MergeList(returnValue, Load(fileName));
            }

            return returnValue;
        }

        private static List<Dictionary<string, string>> MergeList(params List<Dictionary<string, string>>[] list)
        {
            var returnValue = new List<Dictionary<string, string>>();

            foreach (var item in list)
            {
                returnValue.AddRange(item);
            }

            return returnValue;
        }
    }
}
