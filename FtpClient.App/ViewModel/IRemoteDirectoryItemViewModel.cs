using FluentFTP;
using FtpClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FtpClient.Core.Model.Interface;

namespace FtpClient.ViewModel
{
    public interface IRemoteDirectoryItemViewModel : IDirectoryItemViewModel
    {
        IRemoteItem Item { get; set; }
    }
}
