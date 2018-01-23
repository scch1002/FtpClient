using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model
{
    public class DownloadRemoteItem : IDownloadRemoteItem
    {
        public bool OverWrite { get; set; }

        public IRemoteItem Item { get; set; }

        public ILocalDirectory Destination { get; set; }
    }
}
