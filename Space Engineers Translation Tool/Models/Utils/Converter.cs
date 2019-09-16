using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Space_Engineers_Translation_Tool.Models.Objects;

namespace Space_Engineers_Translation_Tool.Models.Utils
{
    class Converter
    {
        public static List<TranslationFile> Convert(List<Dictionary<string, string>> list)
        {
            var returnValue = new List<TranslationFile>();

            foreach (var item in list)
            {
                if (!(item.ContainsKey("Name") && item.ContainsKey("Url") && item.ContainsKey("Path")))
                {
                    // フォーマットが正しくないDictionaryはスキップ。
                    // TODO: ログか何かを残すべき。
                    continue;
                }
                returnValue.Add(new TranslationFile(item["Name"], item["Url"], item["Path"]));
            }

            return returnValue;
        }
    }
}
