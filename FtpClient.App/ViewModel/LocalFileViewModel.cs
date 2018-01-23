using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using FtpClient.Model;
using FtpClient.Core.Model.Interface;
using FtpClient.Core.Model;

namespace FtpClient.ViewModel
{
    public class LocalFileViewModel : ViewModelBase, ILocalDirectoryItemViewModel
    {
        public FileSystemItemType Type => FileSystemItemType.File;

        public string FileName => Path.GetFileNameWithoutExtension(StorageItem.Name);

        public string Extension => Path.GetExtension(StorageItem.Name);

        public string FullFileName => StorageItem.Name;

        public ILocalFile FileData => (ILocalFile)StorageItem;

        public string DisplayName => FullFileName;

        public int Size => 0;

        public ILocalItem StorageItem { get; set; }
    }
}
