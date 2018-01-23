using FtpClient.Message;
using FtpClient.Service;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using FluentFTP;
using Windows.UI.Popups;
using FtpClient.Core.Model;
using FtpClient.ViewModel.Interface;

namespace FtpClient.ViewModel
{
    public class LocalSystemViewModel : ViewModelBase, ILocalSystemViewModel
    {
        public LocalSystemViewModel()
        {
            SetLocalRootDirectoryCommand = new RelayCommand(SetLocalRootDirectory);
        }

        private ILocalWorkingDirectoryViewModel _localWorkingDirectory;
        public ILocalWorkingDirectoryViewModel LocalWorkingDirectory
        {
            get { return _localWorkingDirectory; }
            set
            {
                _localWorkingDirectory = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SetLocalRootDirectoryCommand { get; set; }

        private async void SetLocalRootDirectory()
        {
            var folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            var folder = await folderPicker.PickSingleFolderAsync();

            var localDirectory = new LocalDirectory(folder);

            var items = FtpService.CreatDirectoryItemViewModels(localDirectory);

            LocalWorkingDirectory = new LocalWorkingDirectoryViewModel
            {
                WorkingDirectory = localDirectory,
                Items = new ObservableCollection<ILocalDirectoryItemViewModel>(items)
            };
        }
    }
}
