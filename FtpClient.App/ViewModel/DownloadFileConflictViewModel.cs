using FtpClient.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FtpClient.Core.Model.Interface;
using FtpClient.Core.Model;

namespace FtpClient.ViewModel
{
    public class DownloadFileConflictViewModel : ViewModelBase
    {
        public DownloadFileConflictViewModel()
        {

        }

        public string FullFileName => Item.Item.Name;

        public bool OverWrite
        {
            get { return Item.OverWrite; }
            set
            {
                Item.OverWrite = value;
            }
        }

        public FileSystemItemType Type => FileSystemItemType.File;

        public string DisplayName => Item.Item.Name;

        public int Size => 0;

        public IDownloadRemoteItem Item { get; set; }
    }
}
