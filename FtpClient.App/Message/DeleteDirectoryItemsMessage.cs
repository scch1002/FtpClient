using FluentFTP;
using FtpClient.Core.Model.Interface;
using FtpClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FtpClient.Message
{
    public class DeleteDirectoryItemsMessage
    {
        public List<IRemoteItem> Items { get; set; }
    }
}
