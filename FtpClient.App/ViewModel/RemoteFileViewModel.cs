using FtpClient.Message;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FtpClient.Model;
using System.IO;
using FtpClient.Core.Model.Interface;
using FtpClient.Core.Model;

namespace FtpClient.ViewModel
{
    class RemoteFileViewModel : ViewModelBase, IRemoteDirectoryItemViewModel
    {
        public RemoteFileViewModel()
        {
            ChangePermissionsCommand = new RelayCommand(ChangePermissions);
        }

        public ICommand ChangePermissionsCommand { get; set; }

        public FileSystemItemType Type => FileSystemItemType.File;

        public string FileName => Path.GetFileNameWithoutExtension(Item.Name);

        public string Extension => Path.GetExtension(Item.Name);

        public string FullFileName => Item.Name;

        public IRemoteItem FtpListItem => Item;

        public string DisplayName => FullFileName;

        public int Size => 0;

        public IRemoteItem Item { get; set; }

        public void ChangePermissions()
        {
            MessengerInstance.Send(new ChangePermissionsMessage
            {
                Path = FtpListItem.FullName,
                Group = FluentFTP.FtpPermission.Read,
                Other = FluentFTP.FtpPermission.Read,
                Owner = FluentFTP.FtpPermission.Read,
            });
        }
    }
}
