using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;

namespace Space_Engineers_Translation_Tool.Models.Objects
{
    class TranslationSource
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string EnglishValue { get; set; }
    }
}
