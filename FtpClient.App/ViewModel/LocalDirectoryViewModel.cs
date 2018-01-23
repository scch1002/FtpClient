using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using FtpClient.Model;
using FtpClient.Core.Model.Interface;
using FtpClient.Core.Model;

namespace FtpClient.ViewModel
{
    public class LocalDirectoryViewModel : ViewModelBase, ILocalDirectoryItemViewModel
    {
        public FileSystemItemType Type => FileSystemItemType.Directory;

        public ILocalDirectory StorageFolder => (ILocalDirectory)StorageItem;

        public string DirectoryName => StorageItem.Name;

        public string DisplayName => DirectoryName;

        public int Size => 0;

        public ILocalItem StorageItem { get; set; }
    }
}
