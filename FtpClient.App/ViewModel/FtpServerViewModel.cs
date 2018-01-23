using FluentFTP;
using FtpClient.Core.Model.Interface;
using FtpClient.Message;
using FtpClient.Model;
using FtpClient.Model.FtpAction;
using FtpClient.Service;
using FtpClient.ViewModel.Interface;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Popups;
using FtpClient.Core;
using FtpClient.Core.Model.FtpAction.Interface;

namespace FtpClient.ViewModel
{
    public class FtpServerViewModel : ViewModelBase, IFtpServerViewModel
    {
        public FtpServerViewModel(
            IResolve dependencyService,
            IFtpServer ftpServer)
        {
            DependencyService = dependencyService;
            FtpServer = ftpServer;
            ConnectToServerCommand = new RelayCommand(InitialConnectToServer);

            MessengerInstance.Register<ChangeWorkingDirectoryMessage>(this, SetWorkingDirectory);
        }

        public ICommand ConnectToServerCommand { get; set; }

        public string ServerUrl
        {
            get { return FtpServer.ServerUrl; }
            set
            {
                FtpServer.ServerUrl = value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get { return FtpServer.UserName; }
            set
            {
                FtpServer.UserName = value;
                RaisePropertyChanged();
            }
        }

        public string UserPassword
        {
            get { return FtpServer.UserPassword; }
            set
            {
                FtpServer.UserPassword = value;
                RaisePropertyChanged();
            }
        }

        public IFtpServer FtpServer { get; set; }

        private IRemoteWorkingDirectoryViewModel _remoteWorkingDirectory;
        public IRemoteWorkingDirectoryViewModel RemoteWorkingDirectory
        {
            get { return _remoteWorkingDirectory; }
            set
            {
                _remoteWorkingDirectory = value;
                RaisePropertyChanged();
            }
        }

        public IResolve DependencyService { get; set; }

        private void RefreshWorkingDirectory(string path)
        {
            var ftpAction = DependencyService.Resolve<IRetrieveDirectory>();

            ftpAction.Path = path;
            ftpAction.FtpServer = FtpServer;

            ftpAction.Execute();

            if (ftpAction.FailedToConnectToServer)
            {
                FailedToConnectToServer();
            }
            else
            {
                var remoteWorkingDirectoryViewModel = DependencyService.Resolve<IRemoteWorkingDirectoryViewModel>();

                remoteWorkingDirectoryViewModel.ServerPath = path;
                remoteWorkingDirectoryViewModel.Items = new ObservableCollection<IRemoteDirectoryItemViewModel>(
                        FtpService.CreatDirectoryItemViewModels(ftpAction.DirectoryItems));

                RemoteWorkingDirectory = remoteWorkingDirectoryViewModel;
            }
        }

        private void InitialConnectToServer()
        {
            RefreshWorkingDirectory("/");
        }

        private void SetWorkingDirectory(ChangeWorkingDirectoryMessage message)
        {
            var changeWorkingDirectory = DependencyService.Resolve<IChangeWorkingDirectory>();
            changeWorkingDirectory.FtpServer = FtpServer;
            changeWorkingDirectory.NewWorkingDirectoryPath = message.Path;

            changeWorkingDirectory.Execute();

            if (changeWorkingDirectory.FailedToConnectToServer)
            {
                FailedToConnectToServer();
                return;
            }

            RefreshWorkingDirectory(message.Path);
        }

        private void FailedToConnectToServer()
        {
            MessengerInstance.Send(new ApplicationStatusMessage
            {
                Message = "Failed to connect to server."
            });
        }
    }
}
