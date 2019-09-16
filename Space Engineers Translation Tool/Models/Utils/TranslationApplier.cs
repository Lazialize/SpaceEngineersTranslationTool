using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CsvHelper;

namespace Space_Engineers_Translation_Tool.Models.Utils
{
    class TranslationApplier
    {
        private readonly XmlDocument xmlDocument;
        private readonly string xmlFilePath;

        public TranslationApplier(string filePath)
        {
            xmlDocument = new XmlDocument
            {
                PreserveWhitespace = true
            };
            xmlDocument.Load(filePath);
            xmlFilePath = filePath;
        }

        /// <summary>
        /// 指定のkeyを持つノードの値をvalueに書き換えます。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException">指定のキーのノードが存在しない場合に発生します。</exception>
        public void Replace(string key, string value)
        {
            XmlNode xmlNode = xmlDocument.SelectSingleNode($"/root/data[@name='{key}']/value");

            if (xmlNode is null)
            {
                throw new ArgumentException();
            }

            xmlNode.InnerText = value;
        }

        public void Save()
        {
            xmlDocument.Save(xmlFilePath);
        }
    }
}
