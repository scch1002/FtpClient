using FtpClient.Core.Model.Interface;
using FtpClient.Message;
using FtpClient.ViewModel.Interface;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace FtpClient.ViewModel
{
    public class LocalWorkingDirectoryViewModel : ViewModelBase, ILocalWorkingDirectoryViewModel
    {
        public LocalWorkingDirectoryViewModel()
        {
            UploadSelectedCommand = new RelayCommand(UploadSelected);
        }

        public ICommand UploadSelectedCommand { get; set; }

        public ILocalDirectory WorkingDirectory { get; set; }

        public ObservableCollection<ILocalDirectoryItemViewModel> SelectedItems { get; set; } = new ObservableCollection<ILocalDirectoryItemViewModel>();

        public ObservableCollection<ILocalDirectoryItemViewModel> Items { get; set; }

        private void UploadSelected()
        {
            MessengerInstance.Send(new UploadDirectoryItemsMessage
            {
                LocalItems = SelectedItems.Select(s => s.StorageItem).ToList()
            });
        }
    }
}
