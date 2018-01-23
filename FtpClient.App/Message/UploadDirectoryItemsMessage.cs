using FtpClient.Model;
using FtpClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using FtpClient.Core.Model.Interface;

namespace FtpClient.Message
{
    public class UploadDirectoryItemsMessage
    {
        public bool ResolveConflicts { get; set; }

        public ICollection<ILocalItem> LocalItems { get; set; }
    }
}
