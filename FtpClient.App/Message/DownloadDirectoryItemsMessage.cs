using FluentFTP;
using FtpClient.Model;
using FtpClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FtpClient.Core.Model.Interface;

namespace FtpClient.Message
{
    public class DownloadDirectoryItemsMessage
    {
        public bool ResolveConflicts { get; set; }

        public List<IRemoteItem> Items { get; set; }
    }
}
