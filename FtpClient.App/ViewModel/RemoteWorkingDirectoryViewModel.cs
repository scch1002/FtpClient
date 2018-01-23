using FtpClient.Message;
using FtpClient.ViewModel.Interface;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace FtpClient.ViewModel
{
    public class RemoteWorkingDirectoryViewModel : ViewModelBase, IRemoteWorkingDirectoryViewModel
    {
        public RemoteWorkingDirectoryViewModel()
        {
            DeleteSelectedItemsCommand = new RelayCommand(DeleteSelectedItems);
            DownloadSelectedItemsCommand = new RelayCommand(DownloadSelectedItems);
            NavigateToParentDirectory = new RelayCommand(ChangeWorkingDirectoryToParent);
        }

        public ICommand DeleteSelectedItemsCommand { get; set; }

        public ICommand DownloadSelectedItemsCommand { get; set; }

        public ICommand NavigateToParentDirectory { get; set; }

        public string ServerPath { get; set; }

        public ObservableCollection<IRemoteDirectoryItemViewModel> SelectedItems { get; set; } = new ObservableCollection<IRemoteDirectoryItemViewModel>();

        public ObservableCollection<IRemoteDirectoryItemViewModel> Items { get; set; } = new ObservableCollection<IRemoteDirectoryItemViewModel>();

        public void DeleteSelectedItems()
        {
            var deleteItems = SelectedItems.ToList();
            MessengerInstance.Send(new DeleteDirectoryItemsMessage
            {
                Items = deleteItems.Select(s => s.Item).ToList()
            });
            foreach(var item in deleteItems)
            {
                SelectedItems.Remove(item);
                Items.Remove(item);
            }
        }

        public void DownloadSelectedItems()
        {
            var downloadItems = SelectedItems.ToList();
            MessengerInstance.Send(new DownloadDirectoryItemsMessage
            {
                Items = downloadItems.Select(s => s.Item).ToList()
            });
        }

        private void ChangeWorkingDirectoryToParent()
        {
            MessengerInstance.Send(new ChangeWorkingDirectoryMessage
            {
                Path = Path.GetDirectoryName(ServerPath)
            });
        }
    }
}
