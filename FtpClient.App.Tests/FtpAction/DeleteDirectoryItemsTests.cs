using FtpClient.Core.Model;
using FtpClient.Core.Model.Interface;
using FtpClient.Model.FtpAction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.App.Tests.FtpAction
{
    [TestClass]
    public class DeleteDirectoryItemsTests
    {

        [TestMethod]
        public void Delete_file()
        {
            var ftpServer = new Mock<IFtpServer>();
            var ftpClient = new Mock<IFtpClient>();

            var remoteFile = new Mock<IRemoteFile>();
            var fullName = "{9442849B-2237-4917-8DE8-B728E9496311}";
            remoteFile.SetupGet(s => s.FullName).Returns(fullName);
            remoteFile.SetupGet(s => s.Type).Returns(FileSystemItemType.File);

            var localDirectory = new Mock<ILocalDirectory>();

            ftpServer.Setup(s => s.ConnectToServer())
                .Returns(() => ftpClient.Object);

            var sut = new DeleteDirectoryItems()
            {
                FtpServer = ftpServer.Object,
                Items = new List<IRemoteItem> { remoteFile.Object }
            };
            sut.Execute();

            ftpClient.Verify(v => v.DeleteFile(It.Is<string>(i => i == fullName)));

        }

        [TestMethod]
        public void Delete_directory()
        {
            var ftpServer = new Mock<IFtpServer>();
            var ftpClient = new Mock<IFtpClient>();

            var remoteDirectory = new Mock<IRemoteDirectory>();

            var fullName = "{9442849B-2237-4917-8DE8-B728E9496311}";

            remoteDirectory.SetupGet(s => s.FullName).Returns(fullName);
            remoteDirectory.SetupGet(s => s.Type).Returns(FileSystemItemType.Directory);

            ftpServer.Setup(s => s.ConnectToServer())
                .Returns(() => ftpClient.Object);

            var sut = new DeleteDirectoryItems()
            {
                FtpServer = ftpServer.Object,
                Items = new List<IRemoteItem> { remoteDirectory.Object }
            };
            sut.Execute();

            ftpClient.Verify(v => v.DeleteDirectory(It.Is<string>(i => i == fullName)));
        }
    }
}
