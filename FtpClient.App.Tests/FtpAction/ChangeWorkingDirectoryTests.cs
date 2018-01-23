using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FtpClient.Model.FtpAction;
using FtpClient.Core.Model.Interface;
using Moq;

namespace FtpClient.App.Tests.FtpAction
{
    [TestClass]
    public class ChangeWorkingDirectoryTests
    {
        [TestMethod]
        public void Set_working_directory()
        {
            var ftpServer = new Mock<IFtpServer>();
            var ftpClient = new Mock<IFtpClient>();

            var workingDirectory = "075A6DED-F686-4E04-BD50-C4853E29D97E";

            ftpServer.Setup(s => s.ConnectToServer())
                .Returns(() => ftpClient.Object);

            var sut = new ChangeWorkingDirectory();
            sut.FtpServer = ftpServer.Object;
            sut.NewWorkingDirectoryPath = workingDirectory;
            sut.Execute();

            ftpClient.Verify(v => v.SetWorkingDirectory(It.Is<string>(i => i == workingDirectory)));
        }
    }
}
