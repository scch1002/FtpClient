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
    public class RetrieveDirectoryTests
    {
        [TestMethod]
        public void Retrieve_working_directory()
        {
            var ftpServer = new Mock<IFtpServer>();
            var ftpClient = new Mock<IFtpClient>();

            var remoteFile = new Mock<IRemoteFile>();

            ftpClient.Setup(s => s.GetItems(It.IsAny<string>()))
                .Returns(new IRemoteItem[] { remoteFile.Object });

            ftpServer.Setup(s => s.ConnectToServer())
                .Returns(() => ftpClient.Object);

            var sut = new RetrieveDirectory()
            {
                FtpServer = ftpServer.Object,
                Path = "test"
            };
            sut.Execute();

            ftpClient.Verify(v => v.GetItems(It.Is<string>(i => i == "test")));
            Assert.IsTrue(sut.DirectoryItems.Any());
        }
    }
}
