using FtpClient.Core;
using FtpClient.Core.Model.FtpAction.Interface;
using FtpClient.Core.Model.Interface;
using FtpClient.Message;
using FtpClient.ViewModel;
using FtpClient.ViewModel.Interface;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.App.Tests.ViewModel
{
    [TestClass]
    public class FtpServerViewModelTests
    {
        [TestMethod]
        public void InitialConectToServer_failed_to_connect_send_message()
        {
            var messageSent = false;
            Messenger.Default.Register<ApplicationStatusMessage>(this, testMessage => messageSent = true);

            var dependencyService = new Mock<IResolve>();
            var retrieveDirectory = new Mock<IRetrieveDirectory>();
            var ftpServerViewModel = new Mock<IFtpServerViewModel>();
            var localSystemViewModel = new Mock<ILocalSystemViewModel>();
            var ftpServer = new Mock<IFtpServer>();

            ftpServerViewModel.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            dependencyService.Setup(s => s.Resolve<IRetrieveDirectory>())
                .Returns(retrieveDirectory.Object);

            retrieveDirectory.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);
            retrieveDirectory.SetupGet(s => s.FailedToConnectToServer).Returns(true);

            var sut = new FtpServerViewModel(dependencyService.Object, ftpServer.Object);

            sut.ConnectToServerCommand.Execute(null);

            Assert.IsTrue(messageSent);
        }

        [TestMethod]
        public void InitialConectToServer_retrieved_item_set_to_directory()
        {
            var testItemCollection = new ObservableCollection<IRemoteDirectoryItemViewModel>();
        
            var dependencyService = new Mock<IResolve>();
            var retrieveDirectory = new Mock<IRetrieveDirectory>();
            var ftpServerViewModel = new Mock<IFtpServerViewModel>();
            var localSystemViewModel = new Mock<ILocalSystemViewModel>();
            var ftpServer = new Mock<IFtpServer>();
            var remoteWorkingDirectoryViewModel = new Mock<IRemoteWorkingDirectoryViewModel>();
            var remoteItem = new Mock<IRemoteItem>();

            remoteWorkingDirectoryViewModel.SetupSet(s => s.Items = It.IsAny<ObservableCollection<IRemoteDirectoryItemViewModel>>())
                .Callback<ObservableCollection<IRemoteDirectoryItemViewModel>>(c => testItemCollection = c);

            ftpServerViewModel.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);

            dependencyService.Setup(s => s.Resolve<IRetrieveDirectory>())
                .Returns(retrieveDirectory.Object);

            dependencyService.Setup(s => s.Resolve<IRemoteWorkingDirectoryViewModel>())
                .Returns(remoteWorkingDirectoryViewModel.Object);

            retrieveDirectory.SetupGet(s => s.FtpServer).Returns(ftpServer.Object);
            retrieveDirectory.SetupGet(s => s.FailedToConnectToServer).Returns(false);

            retrieveDirectory.SetupGet(s => s.DirectoryItems).Returns(new List<IRemoteItem> { remoteItem.Object });

            var sut = new FtpServerViewModel(dependencyService.Object, ftpServer.Object);

            sut.ConnectToServerCommand.Execute(null);

            Assert.IsTrue(testItemCollection.Any(a => a.Item == remoteItem.Object));
        }
    }
}
