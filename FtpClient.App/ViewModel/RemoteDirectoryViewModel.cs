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
using FtpClient.Core.Model.Interface;
using FtpClient.Core.Model;

namespace FtpClient.ViewModel
{
    public class RemoteDirectoryViewModel : ViewModelBase, IRemoteDirectoryItemViewModel
    {
        public RemoteDirectoryViewModel()
        {
            SetAsWorkingDirectoryCommand = new RelayCommand(SetAsWorkingDirectory);
        }

        public ICommand SetAsWorkingDirectoryCommand { get; set; }

        public FileSystemItemType Type => FileSystemItemType.Directory;

        public IRemoteItem FtpListItem => Item;

        public string DirectoryName => Item.Name;

        public string DisplayName => DirectoryName;

        public int Size => 0;

        public IRemoteItem Item { get; set; }

        private void SetAsWorkingDirectory()
        {
            MessengerInstance.Send(new ChangeWorkingDirectoryMessage
            {
                Path = FtpListItem.FullName
            });
        }
    }
}
