using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Space_Engineers_Translation_Tool.Models.Utils
{
    class Downloader
    {
        private static WebClient downloadClient = null;
        public static async Task DownloadAsync(Uri uri, string path)
        {

            if (downloadClient == null)
            {
                downloadClient = new WebClient();
            }

            await Task.Run<Task>(() => downloadClient.DownloadFileTaskAsync(uri, path)).Result;
        }
    }
}
