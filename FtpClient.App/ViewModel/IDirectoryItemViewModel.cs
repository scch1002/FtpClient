using FtpClient.Core.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.ViewModel
{
    public interface IDirectoryItemViewModel
    {
        FileSystemItemType Type { get; }

        string DisplayName { get; }

        int Size { get; }
    }
}
