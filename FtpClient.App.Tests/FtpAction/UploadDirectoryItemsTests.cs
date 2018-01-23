using FtpClient.Core;
using FtpClient.Core.Model;
using FtpClient.Core.Model.Interface;
using FtpClient.Model.FtpAction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.App.Tests.FtpAction
{
    [TestClass]
    public class UploadDirectoryItemsTests
    {
        [TestMethod]
        public void Upload_file()
        {
            var dependencyService = new Mock<IResolve>();
            var ftpServer = new Mock<IFtpServer>();
            var ftpClient = new Mock<IFtpClient>();

            ftpServer.Setup(s => s.ConnectToServer())
                .Returns(() => ftpClient.Object);

            var directoryPath = "test";
            var fileName = "testFileName";

            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("test")))
            {
                var localFile = new Mock<ILocalFile>();
                localFile.SetupGet(s => s.Name).Returns(fileName);
                localFile.Setup(s => s.OpenStreamForReadAsync()).Returns(() =>
                {
                    var task = new Task<Stream>(() => test_Stream);
                    task.Start();
                    return task;
                });

                var uploadLocalItem = new Mock<IUploadLocalItem>();
                uploadLocalItem.SetupGet(s => s.DestinationPath).Returns(directoryPath);
                uploadLocalItem.SetupGet(s => s.Item).Returns(localFile.Object);
                uploadLocalItem.SetupGet(s => s.OverWrite).Returns(false);

                var sut = new UploadDirectoryItems(dependencyService.Object)
                {
                    FtpServer = ftpServer.Object,
                    Items = new IUploadLocalItem[] { uploadLocalItem.Object }
                };

                sut.Execute();

                ftpClient.Verify(v => v.Upload(It.IsAny<Stream>(), It.Is<string>(i => i == directoryPath + "/" + fileName), It.Is<bool>(i => i == false)));
            }
        }

        [TestMethod]
        public void Upload_directory()
        {
            var dependencyService = new Mock<IResolve>();
            var ftpServer = new Mock<IFtpServer>();
            var ftpClient = new Mock<IFtpClient>();

            ftpServer.Setup(s => s.ConnectToServer())
                .Returns(() => ftpClient.Object);

            var directoryPath = "test";
            var fileName = "testFileName";

            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("test")))
            {
                var localFile = new Mock<ILocalFile>();
                localFile.SetupGet(s => s.Name).Returns(fileName);
                localFile.Setup(s => s.OpenStreamForReadAsync()).Returns(() =>
                {
                    var task = new Task<Stream>(() => test_Stream);
                    task.Start();
                    return task;
                });

                var directoryName = "testDirectory";
                var localDirectory = new Mock<ILocalDirectory>();
                localDirectory.SetupGet(s => s.Name).Returns(directoryName);
                localDirectory.SetupGet(s => s.Type).Returns(FileSystemItemType.Directory);

                var childDirectoryName = "childDirectoryName";
                var localChildDirectory = new Mock<ILocalDirectory>();
                localChildDirectory.SetupGet(s => s.Name).Returns(childDirectoryName);
                localChildDirectory.SetupGet(s => s.Type).Returns(FileSystemItemType.Directory);

                localDirectory.Setup(s => s.GetFilesAsync()).Returns(() =>
                {
                    var task = new Task<ICollection<ILocalFile>>(() => new ILocalFile[] { localFile.Object });
                    task.Start();
                    return task;
                });

                localDirectory.Setup(s => s.GetFoldersAsync()).Returns(() =>
                {
                    var task = new Task<ICollection<ILocalDirectory>>(() => new ILocalDirectory[] { localChildDirectory.Object });
                    task.Start();
                    return task;
                });


                var uploadLocalItem = new Mock<IUploadLocalItem>();
                uploadLocalItem.SetupGet(s => s.DestinationPath).Returns(directoryPath);
                uploadLocalItem.SetupGet(s => s.Item).Returns(localDirectory.Object);
                uploadLocalItem.SetupGet(s => s.OverWrite).Returns(false);

                var sut = new UploadDirectoryItems(dependencyService.Object)
                {
                    FtpServer = ftpServer.Object,
                    Items = new IUploadLocalItem[] { uploadLocalItem.Object }
                };

                sut.Execute();

                ftpClient.Verify(v => v.CreateDirectory(directoryPath + "/" + directoryName));
                ftpClient.Verify(v => v.CreateDirectory(directoryPath + "/" + directoryName + "/" + childDirectoryName));
                ftpClient.Verify(v => v.Upload(It.IsAny<Stream>(), It.Is<string>(i => i == directoryPath + "/" + directoryName + "/" + fileName), It.Is<bool>(i => i == false)));
            }
        }

        [TestMethod]
        public void Handle_existing_file_conflict()
        {
            var dependencyService = new Mock<IResolve>();
            var ftpServer = new Mock<IFtpServer>();
            var ftpClient = new Mock<IFtpClient>();

            ftpServer.Setup(s => s.ConnectToServer())
                .Returns(() => ftpClient.Object);

            var directoryPath = "test";
            var fileName = "testFileName";

            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("test")))
            {
                var localFile = new Mock<ILocalFile>();
                localFile.SetupGet(s => s.Name).Returns(fileName);
                localFile.Setup(s => s.OpenStreamForReadAsync()).Returns(() =>
                {
                    var task = new Task<Stream>(() => test_Stream);
                    task.Start();
                    return task;
                });

                var newUploadLocalItem = new Mock<IUploadLocalItem>();

                var resolutionService = new Mock<IResolve>();
                resolutionService.Setup(s => s.Resolve<IUploadLocalItem>())
                .Returns(newUploadLocalItem.Object);

                var uploadLocalItem = new Mock<IUploadLocalItem>();
                uploadLocalItem.SetupGet(s => s.DestinationPath).Returns(directoryPath);
                uploadLocalItem.SetupGet(s => s.Item).Returns(localFile.Object);
                uploadLocalItem.SetupGet(s => s.OverWrite).Returns(false);

                ftpClient.Setup(s => s.FileExists(It.Is<string>(i => i == directoryPath + "/" + fileName)))
                    .Returns(true);

                var sut = new UploadDirectoryItems(dependencyService.Object)
                {
                    FtpServer = ftpServer.Object,
                    Items = new IUploadLocalItem[] { uploadLocalItem.Object },
                    DependencyService = resolutionService.Object
                };

                sut.Execute();
                                
                resolutionService.Verify(v => v.Resolve<IUploadLocalItem>());
                Assert.IsTrue(sut.FileConflicts.Any());
            }
        }
    }
}
