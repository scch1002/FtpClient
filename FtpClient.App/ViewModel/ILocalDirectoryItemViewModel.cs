using FtpClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using FtpClient.Core.Model.Interface;

namespace FtpClient.ViewModel
{
    public interface ILocalDirectoryItemViewModel : IDirectoryItemViewModel
    {
        ILocalItem StorageItem { get; set; }
    }
}
