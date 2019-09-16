using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Engineers_Translation_Tool.Models.Objects
{
    class TranslationSourceMap : CsvHelper.Configuration.ClassMap<TranslationSource>
    {
        public TranslationSourceMap()
        {
            Map(x => x.Key).Index(0);
            Map(x => x.EnglishValue).Index(1);
            Map(x => x.Value).Index(2);
        }
    }
}
