using FluentFTP;
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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using FtpClient.Model.FtpAction;
using FtpClient.Model;
using FtpClient.Core.Model.Interface;
using FtpClient.Core;
using FtpClient.ViewModel.Interface;
using FtpClient.Core.Model.FtpAction.Interface;

namespace FtpClient.ViewModel
{
    public class MainViewModel : ViewModelBase, IDependencyServiceProperty
    {
        public MainViewModel(
            IResolve dependencyService,
            IFtpServerViewModel ftpServerViewModel,
            ILocalSystemViewModel localSystem)
        {
            DependencyService = dependencyService;
            FtpServer = ftpServerViewModel;
            LocalSystem = localSystem;
            MessengerInstance.Register<UploadDirectoryItemsMessage>(this, message => UploadDirectoryItems(message));
            MessengerInstance.Register<DeleteDirectoryItemsMessage>(this, message => DeleteDirectoryItems(message));
            MessengerInstance.Register<ResolveUploadConflictsMessage>(this, message => ResolveUploadConflicts(message));
            MessengerInstance.Register<ResolveDownloadConflictsMessage>(this, message => ResolveDownloadConflicts(message));
            MessengerInstance.Register<ApplicationStatusMessage>(this, message => ApplicationStatus(message));
        }

        public IResolve DependencyService { get; set; }

        public IFtpServerViewModel FtpServer { get; set; }

        public ILocalSystemViewModel LocalSystem { get; set; }

        public Task DownloadDirectoryItems(DownloadDirectoryItemsMessage message)
        {
            return Task.Run(() =>
            {
                var downloadItems = CreateDownloadDirectoryItemsAction(message.Items);

                DownloadItems(downloadItems, false);
            });
        }

        public Task DeleteDirectoryItems(DeleteDirectoryItemsMessage message)
        {
            return Task.Run(() =>
            {
                var deleteItems = DependencyService.Resolve<IDeleteDirectoryItems>();
                deleteItems.Items = message.Items;
                deleteItems.FtpServer = FtpServer.FtpServer;

                deleteItems.Execute();

                if (deleteItems.FailedToConnectToServer)
                {
                    FailedToConnectToServer();
                    return;
                }

                MessengerInstance.Send(new ApplicationStatusMessage
                {
                    Message = "Delete Complete"
                });
            });
        }

        public Task UploadDirectoryItems(UploadDirectoryItemsMessage message)
        {
            return Task.Run(() =>
            {
                var uploadItems = CreateUploadDirectoryItemsAction(message.LocalItems);

                UploadItems(uploadItems, false);
            });
        }

        public Task ResolveDownloadConflicts(ResolveDownloadConflictsMessage message)
        {
            return Task.Run(() =>
            {
                var downloadItems = new DownloadDirectoryItems
                {
                    FtpServer = FtpServer.FtpServer,
                    Items = message.LocalItems
                };

                DownloadItems(downloadItems, true);
            });
        }

        public Task ResolveUploadConflicts(ResolveUploadConflictsMessage message)
        {
            return Task.Run(() =>
            {
                var uploadItems = DependencyService.Resolve<IUploadDirectoryItems>();
                uploadItems.FtpServer = FtpServer.FtpServer;
                uploadItems.Items = message.LocalItems;

                UploadItems(uploadItems, true);
            });
        }

        public Task ApplicationStatus(ApplicationStatusMessage message)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               async () =>
               {
                   var dialog = new MessageDialog(message.Message);
                   await dialog.ShowAsync();
               }).AsTask();
        }

        private void FailedToConnectToServer()
        {
            MessengerInstance.Send(new ApplicationStatusMessage
            {
                Message = "Failed to connect to server."
            });
        }

        private void ResolveUploadConflicts(List<IUploadLocalItem> conflicts)
        {
            var uploadConflictsViewModel = new UploadFileConflictsViewModel();

            uploadConflictsViewModel.Conflicts.AddRange(conflicts.Select(s => new UploadFileConflictViewModel
            {
                StorageItem = s
            }));

            MessengerInstance.Send(new ChangePageMessage
            {
                ViewModel = uploadConflictsViewModel
            });
        }

        private void ResolveDownloadConflicts(List<IDownloadRemoteItem> conflicts)
        {
            var downloadConflictsViewModel = new DownloadFileConflictsViewModel();

            downloadConflictsViewModel.Conflicts.AddRange(conflicts.Select(s => new DownloadFileConflictViewModel
            {
                Item = s
            }));

            MessengerInstance.Send(new ChangePageMessage
            {
                ViewModel = downloadConflictsViewModel
            });
        }

        private IUploadDirectoryItems CreateUploadDirectoryItemsAction(ICollection<ILocalItem> localItems)
        {
            var uploadItems = DependencyService.Resolve<IUploadDirectoryItems>();
            uploadItems.FtpServer = FtpServer.FtpServer;

            foreach (var item in localItems)
            {
                var uploadLocalItem = DependencyService.Resolve<IUploadLocalItem>();
                uploadLocalItem.Item = item;
                uploadLocalItem.DestinationPath = FtpServer.RemoteWorkingDirectory.ServerPath;
                uploadItems.Items.Add(uploadLocalItem);
            }

            return uploadItems;
        }

        private IDownloadDirectoryItems CreateDownloadDirectoryItemsAction(ICollection<IRemoteItem> remoteItems)
        {
            var downloadItems = DependencyService.Resolve<IDownloadDirectoryItems>();
            downloadItems.FtpServer = FtpServer.FtpServer;

            foreach (var item in remoteItems)
            {
                var downloadRemoteItem = DependencyService.Resolve<IDownloadRemoteItem>();
                downloadRemoteItem.Item = item;
                downloadRemoteItem.Destination = LocalSystem.LocalWorkingDirectory.WorkingDirectory;
                downloadItems.Items.Add(downloadRemoteItem);
            }

            return downloadItems;
        }

        private void UploadItems(IUploadDirectoryItems uploadItems, bool resolveConficts)
        {
            uploadItems.Execute();

            if (uploadItems.FailedToConnectToServer)
            {
                FailedToConnectToServer();
                return;
            }

            if (uploadItems.FileConflicts != null && uploadItems.FileConflicts.Any())
            {
                ResolveUploadConflicts(uploadItems.FileConflicts);
                return;
            }

            if (resolveConficts)
            {
                MessengerInstance.Send(new ChangePageMessage());
            }

            MessengerInstance.Send(new ApplicationStatusMessage
            {
                Message = "Upload Complete"
            });
        }

        private void DownloadItems(IDownloadDirectoryItems downloadItems, bool resolveConficts)
        {
            downloadItems.Execute();

            if (downloadItems.FailedToConnectToServer)
            {
                FailedToConnectToServer();
                return;
            }

            if (downloadItems.FileConflicts != null && downloadItems.FileConflicts.Any())
            {
                ResolveDownloadConflicts(downloadItems.FileConflicts);
                return;
            }

            if (resolveConficts)
            {
                MessengerInstance.Send(new ChangePageMessage());
            }

            MessengerInstance.Send(new ApplicationStatusMessage
            {
                Message = "Download Complete"
            });
        }
    }
}
