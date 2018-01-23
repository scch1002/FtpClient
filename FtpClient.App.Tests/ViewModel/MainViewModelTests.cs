using FtpClient.Core;
using FtpClient.Core.Model.FtpAction.Interface;
using FtpClient.Core.Model.Interface;
using FtpClient.Message;
using FtpClient.Model.FtpAction;
using FtpClient.ViewModel;
using FtpClient.ViewModel.Interface;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.App.Tests.ViewModel
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void DownloadDirectoryItems_failed_to_connect_message_sent()
        {
            var messageSent = false;
            Messenger.Default.Register<ApplicationStatusMessage>(this, testMessage => messageSent = true);

            var dependencyService = new Mock<IResolve>();
            var downloadDirectoryItems = new Mock<IDownloadDirectoryItems>();
            var ftpServerViewModel = new Mock<IFtpServerViewModel>();
            var localSystemViewModel = new Mock<ILocalSystemViewModel>();
            var ftpServer = new Mock<IFtpServer>();

            ftpServerViewModel.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            dependencyService.Setup(s => s.Resolve<IDownloadDirectoryItems>())
                .Returns(downloadDirectoryItems.Object);

            downloadDirectoryItems.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);
            downloadDirectoryItems.SetupGet(s => s.FailedToConnectToServer).Returns(true);

            var message = new DownloadDirectoryItemsMessage
            {
                Items = new List<IRemoteItem>()
            };

            var sut = new MainViewModel(dependencyService.Object, ftpServerViewModel.Object, localSystemViewModel.Object);

            sut.DownloadDirectoryItems(message).Wait();

            Assert.IsTrue(messageSent);
        }

        [TestMethod]
        public void DownloadDirectoryItems_conflicts_message_sent()
        {
            DownloadFileConflictsViewModel downloadFileConflictsViewModel = null;
            var dependencyService = new Mock<IResolve>();
            var downloadDirectoryItems = new Mock<IDownloadDirectoryItems>();
            var ftpServerViewModel = new Mock<IFtpServerViewModel>();
            var localSystemViewModel = new Mock<ILocalSystemViewModel>();
            var ftpServer = new Mock<IFtpServer>();
            var downloadRemoteItem = new Mock<IDownloadRemoteItem>();

            ftpServerViewModel.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            dependencyService.Setup(s => s.Resolve<IDownloadDirectoryItems>())
                .Returns(downloadDirectoryItems.Object);

            downloadDirectoryItems.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);
            downloadDirectoryItems.SetupGet(s => s.FileConflicts).Returns(new List<IDownloadRemoteItem> { downloadRemoteItem.Object });

            Messenger.Default.Register<ChangePageMessage>(this, testMessage => 
            {
                var viewModel = testMessage.ViewModel as DownloadFileConflictsViewModel;
                if (viewModel == null)
                {
                    return;
                }
                downloadFileConflictsViewModel = viewModel;
            });

            var message = new DownloadDirectoryItemsMessage
            {
                Items = new List<IRemoteItem>()
            };

            var sut = new MainViewModel(dependencyService.Object, ftpServerViewModel.Object, localSystemViewModel.Object);

            sut.DownloadDirectoryItems(message).Wait();

            Assert.IsTrue(downloadFileConflictsViewModel.Conflicts.Any(a => a.Item == downloadRemoteItem.Object));
        }

        [TestMethod]
        public void DownloadDirectoryItems_confirmation_message_sent()
        {
            var messageSent = false;
            var dependencyService = new Mock<IResolve>();
            var downloadDirectoryItems = new Mock<IDownloadDirectoryItems>();
            var ftpServerViewModel = new Mock<IFtpServerViewModel>();
            var localSystemViewModel = new Mock<ILocalSystemViewModel>();
            var ftpServer = new Mock<IFtpServer>();
            var downloadRemoteItem = new Mock<IDownloadRemoteItem>();

            ftpServerViewModel.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            dependencyService.Setup(s => s.Resolve<IDownloadDirectoryItems>())
                .Returns(downloadDirectoryItems.Object);

            downloadDirectoryItems.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            Messenger.Default.Register<ApplicationStatusMessage>(this, testMessage => messageSent = true);

            var message = new DownloadDirectoryItemsMessage
            {
                Items = new List<IRemoteItem>()
            };

            var sut = new MainViewModel(dependencyService.Object, ftpServerViewModel.Object, localSystemViewModel.Object);

            sut.DownloadDirectoryItems(message).Wait();

            Assert.IsTrue(messageSent);
        }

        [TestMethod]
        public void UploadDirectoryItems_failed_to_connect_message_sent()
        {
            var messageSent = false;
            Messenger.Default.Register<ApplicationStatusMessage>(this, testMessage => messageSent = true);

            var dependencyService = new Mock<IResolve>();
            var downloadDirectoryItems = new Mock<IUploadDirectoryItems>();
            var ftpServerViewModel = new Mock<IFtpServerViewModel>();
            var localSystemViewModel = new Mock<ILocalSystemViewModel>();
            var ftpServer = new Mock<IFtpServer>();

            ftpServerViewModel.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            dependencyService.Setup(s => s.Resolve<IUploadDirectoryItems>())
                .Returns(downloadDirectoryItems.Object);

            downloadDirectoryItems.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);
            downloadDirectoryItems.SetupGet(s => s.FailedToConnectToServer).Returns(true);

            var message = new UploadDirectoryItemsMessage
            {
                LocalItems = new List<ILocalItem>()
            };

            var sut = new MainViewModel(dependencyService.Object, ftpServerViewModel.Object, localSystemViewModel.Object);

            sut.UploadDirectoryItems(message).Wait();

            Assert.IsTrue(messageSent);
        }

        [TestMethod]
        public void UploadDirectoryItems_conflicts_message_sent()
        {
            UploadFileConflictsViewModel downloadFileConflictsViewModel = null;
            var dependencyService = new Mock<IResolve>();
            var downloadDirectoryItems = new Mock<IUploadDirectoryItems>();
            var ftpServerViewModel = new Mock<IFtpServerViewModel>();
            var localSystemViewModel = new Mock<ILocalSystemViewModel>();
            var ftpServer = new Mock<IFtpServer>();
            var downloadRemoteItem = new Mock<IUploadLocalItem>();

            ftpServerViewModel.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            dependencyService.Setup(s => s.Resolve<IUploadDirectoryItems>())
                .Returns(downloadDirectoryItems.Object);

            downloadDirectoryItems.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);
            downloadDirectoryItems.SetupGet(s => s.FileConflicts).Returns(new List<IUploadLocalItem> { downloadRemoteItem.Object });

            Messenger.Default.Register<ChangePageMessage>(this, testMessage =>
            {
                var viewModel = testMessage.ViewModel as UploadFileConflictsViewModel;
                if (viewModel == null)
                {
                    return;
                }
                downloadFileConflictsViewModel = viewModel;
            });

            var message = new UploadDirectoryItemsMessage
            {
                LocalItems = new List<ILocalItem>()
            };

            var sut = new MainViewModel(dependencyService.Object, ftpServerViewModel.Object, localSystemViewModel.Object);

            sut.UploadDirectoryItems(message).Wait();

            Assert.IsTrue(downloadFileConflictsViewModel.Conflicts.Any(a => a.StorageItem == downloadRemoteItem.Object));
        }

        [TestMethod]
        public void UploadDirectoryItems_confirmation_message_sent()
        {
            var messageSent = false;
            var dependencyService = new Mock<IResolve>();
            var downloadDirectoryItems = new Mock<IUploadDirectoryItems>();
            var ftpServerViewModel = new Mock<IFtpServerViewModel>();
            var localSystemViewModel = new Mock<ILocalSystemViewModel>();
            var ftpServer = new Mock<IFtpServer>();

            ftpServerViewModel.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            dependencyService.Setup(s => s.Resolve<IUploadDirectoryItems>())
                .Returns(downloadDirectoryItems.Object);

            downloadDirectoryItems.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            Messenger.Default.Register<ApplicationStatusMessage>(this, testMessage => messageSent = true);

            var message = new UploadDirectoryItemsMessage
            {
                LocalItems = new List<ILocalItem>()
            };

            var sut = new MainViewModel(dependencyService.Object, ftpServerViewModel.Object, localSystemViewModel.Object);

            sut.UploadDirectoryItems(message).Wait();

            Assert.IsTrue(messageSent);
        }

        [TestMethod]
        public void DeleteDirectoryItems_failed_to_connect_message_sent()
        {
            var messageSent = false;
            Messenger.Default.Register<ApplicationStatusMessage>(this, testMessage => messageSent = true);

            var dependencyService = new Mock<IResolve>();
            var downloadDirectoryItems = new Mock<IDeleteDirectoryItems>();
            var ftpServerViewModel = new Mock<IFtpServerViewModel>();
            var localSystemViewModel = new Mock<ILocalSystemViewModel>();
            var ftpServer = new Mock<IFtpServer>();

            ftpServerViewModel.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            dependencyService.Setup(s => s.Resolve<IDeleteDirectoryItems>())
                .Returns(downloadDirectoryItems.Object);

            downloadDirectoryItems.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);
            downloadDirectoryItems.SetupGet(s => s.FailedToConnectToServer).Returns(true);

            var message = new DeleteDirectoryItemsMessage
            {
                Items = new List<IRemoteItem>()
            };

            var sut = new MainViewModel(dependencyService.Object, ftpServerViewModel.Object, localSystemViewModel.Object);

            sut.DeleteDirectoryItems(message).Wait();

            Assert.IsTrue(messageSent);
        }

        [TestMethod]
        public void DeleteDirectoryItems_confirmation_message_sent()
        {
            var messageSent = false;
            var dependencyService = new Mock<IResolve>();
            var downloadDirectoryItems = new Mock<IDeleteDirectoryItems>();
            var ftpServerViewModel = new Mock<IFtpServerViewModel>();
            var localSystemViewModel = new Mock<ILocalSystemViewModel>();
            var ftpServer = new Mock<IFtpServer>();

            ftpServerViewModel.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            dependencyService.Setup(s => s.Resolve<IDeleteDirectoryItems>())
                .Returns(downloadDirectoryItems.Object);

            downloadDirectoryItems.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            Messenger.Default.Register<ApplicationStatusMessage>(this, testMessage => messageSent = true);

            var message = new DeleteDirectoryItemsMessage
            {
                Items = new List<IRemoteItem>()
            };

            var sut = new MainViewModel(dependencyService.Object, ftpServerViewModel.Object, localSystemViewModel.Object);

            sut.DeleteDirectoryItems(message).Wait();

            Assert.IsTrue(messageSent);
        }
    }
}
