using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FtpClient.Core.Model.Interface
{
    public interface IDownloadRemoteItem
    {
        bool OverWrite { get; set; }

        IRemoteItem Item { get; set; }

        ILocalDirectory Destination { get; set; }
    }
}
