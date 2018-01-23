using FluentFTP;
using FtpClient.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using FtpClient.Core.Model.Interface;
using FtpClient.Core.Model;

namespace FtpClient.ViewModel
{
    public class UploadFileConflictViewModel : ViewModelBase
    {
        public UploadFileConflictViewModel()
        {

        }

        public string FullFileName => StorageItem.Item.Name;

        public bool OverWrite
        {
            get { return StorageItem.OverWrite; }
            set
            {
                StorageItem.OverWrite = value;
            }
        }

        public FileSystemItemType Type => FileSystemItemType.File;

        public string DisplayName => StorageItem.Item.Name;

        public int Size => 0;

        public IUploadLocalItem StorageItem { get; set; }
    }
}
