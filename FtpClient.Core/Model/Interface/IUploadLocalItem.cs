using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FtpClient.Core.Model.Interface
{
    public interface IUploadLocalItem
    {
        ILocalItem Item { get; set; }

        string DestinationPath { get; set; }

        bool OverWrite { get; set; }
    }
}
