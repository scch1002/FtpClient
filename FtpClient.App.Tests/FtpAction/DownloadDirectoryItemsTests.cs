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
    public class DownloadDirectoryItemsTests
    {
        [TestMethod]
        public void Download_file()
        {
            var ftpServer = new Mock<IFtpServer>();
            var ftpClient = new Mock<IFtpClient>();

            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("test")))
            {
                var localFile = new Mock<ILocalFile>();
                localFile.Setup(s => s.OpenStreamForWriteAsync()).Returns(() =>
                {
                    var task = new Task<Stream>(() => test_Stream);
                    task.Start();
                    return task;
                });

                var remoteFile = new Mock<IRemoteFile>();
                var fullName = "{9442849B-2237-4917-8DE8-B728E9496311}";
                var name = "fileName";
                remoteFile.SetupGet(s => s.FullName).Returns(fullName);
                remoteFile.SetupGet(s => s.Name).Returns(name);
                remoteFile.SetupGet(s => s.Type).Returns(FileSystemItemType.File);

                var localDirectory = new Mock<ILocalDirectory>();

                localDirectory.Setup(s => s.CreateFileAsync(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(() => {
                            var task = new Task<ILocalFile>(() => localFile.Object);
                            task.Start();
                            return task;
                        });

                var downloadRemoteItem = new Mock<IDownloadRemoteItem>();
                downloadRemoteItem.SetupGet(s => s.Item).Returns(remoteFile.Object);
                downloadRemoteItem.SetupGet(s => s.Destination).Returns(localDirectory.Object);

                ftpServer.Setup(s => s.ConnectToServer())
                    .Returns(() => ftpClient.Object);

                var sut = new DownloadDirectoryItems()
                {
                    FtpServer = ftpServer.Object,
                    Items = new List<IDownloadRemoteItem> { downloadRemoteItem.Object }
                };
                sut.Execute();

                ftpClient.Verify(v => v.Download(It.IsAny<Stream>(), It.Is<string>(i => i == fullName)));
                localDirectory.Verify(v => v.TryGetItemAsync(It.Is<string>(i => i == name)));
                localDirectory.Verify(v => v.CreateFileAsync(It.Is<string>(i => i == name), It.Is<bool>(i => i == false)));
                localFile.Verify(v => v.OpenStreamForWriteAsync());
            }
        }

        [TestMethod]
        public void Download_directory()
        {
            var ftpServer = new Mock<IFtpServer>();
            var ftpClient = new Mock<IFtpClient>();

            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("test")))
            {
                var localFile = new Mock<ILocalFile>();
                localFile.Setup(s => s.OpenStreamForWriteAsync()).Returns(() =>
                {
                    var task = new Task<Stream>(() => test_Stream);
                    task.Start();
                    return task;
                });

                var remoteFile = new Mock<IRemoteFile>();
                var fullName = "{9442849B-2237-4917-8DE8-B728E9496311}";
                var name = "fileName";
                remoteFile.SetupGet(s => s.FullName).Returns(fullName);
                remoteFile.SetupGet(s => s.Name).Returns(name);
                remoteFile.SetupGet(s => s.Type).Returns(FileSystemItemType.File);

                var localDirectory = new Mock<ILocalDirectory>();

                var localDirectoryChild = new Mock<ILocalDirectory>();

                localDirectoryChild.Setup(s => s.CreateFileAsync(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(() => {
                        var task = new Task<ILocalFile>(() => localFile.Object);
                        task.Start();
                        return task;
                    });

                localDirectory.Setup(s => s.CreateFolderAsync(It.IsAny<string>()))
                    .Returns(() => {
                        var task = new Task<ILocalDirectory>(() => localDirectoryChild.Object);
                        task.Start();
                        return task;
                    });

                var remoteDirectory = new Mock<IRemoteDirectory>();
                var directoryFullName = "directoryPath";
                remoteDirectory.SetupGet(s => s.FullName).Returns(directoryFullName);
                remoteDirectory.SetupGet(s => s.Name).Returns(directoryFullName);
                remoteDirectory.SetupGet(s => s.Type).Returns(FileSystemItemType.Directory);

                var remoteDirectoryChild = new Mock<IRemoteDirectory>();
                var directoryChildFullName = "directoryPathChild";
                remoteDirectoryChild.SetupGet(s => s.FullName).Returns(directoryChildFullName);
                remoteDirectoryChild.SetupGet(s => s.Name).Returns(directoryChildFullName);
                remoteDirectoryChild.SetupGet(s => s.Type).Returns(FileSystemItemType.Directory);

                var downloadRemoteItem = new Mock<IDownloadRemoteItem>();
                downloadRemoteItem.SetupGet(s => s.Item).Returns(remoteDirectory.Object);
                downloadRemoteItem.SetupGet(s => s.Destination).Returns(localDirectory.Object);

                ftpClient.Setup(s => s.GetItems(It.Is<string>(i => i == directoryFullName)))
                    .Returns(new IRemoteItem[] { remoteDirectoryChild.Object, remoteFile.Object });

                ftpServer.Setup(s => s.ConnectToServer())
                    .Returns(() => ftpClient.Object);

                var sut = new DownloadDirectoryItems()
                {
                    FtpServer = ftpServer.Object,
                    Items = new List<IDownloadRemoteItem> { downloadRemoteItem.Object }
                };
                sut.Execute();

                ftpClient.Verify(v => v.GetItems(It.Is<string>(i => i == directoryFullName)));
                localDirectory.Verify(v => v.CreateFolderAsync(It.Is<string>(i => i == directoryFullName)));
                localDirectoryChild.Verify(v => v.CreateFolderAsync(It.Is<string>(i => i == directoryChildFullName)));
                localDirectoryChild.Verify(v => v.CreateFileAsync(It.Is<string>(i => i == name), It.Is<bool>(i => i == false)));
            }
        }

        [TestMethod]
        public void Handle_existing_file_conflict()
        {
            var ftpServer = new Mock<IFtpServer>();
            var ftpClient = new Mock<IFtpClient>();

            var localFile = new Mock<ILocalFile>();

            var remoteFile = new Mock<IRemoteFile>();
            var fullName = "{9442849B-2237-4917-8DE8-B728E9496311}";
            var name = "fileName";
            remoteFile.SetupGet(s => s.FullName).Returns(fullName);
            remoteFile.SetupGet(s => s.Name).Returns(name);
            remoteFile.SetupGet(s => s.Type).Returns(FileSystemItemType.File);

            var localDirectory = new Mock<ILocalDirectory>();

            localDirectory.Setup(s => s.TryGetItemAsync(It.Is<string>(i => i == name)))
                .Returns(() => {
                    var task = new Task<ILocalItem>(() => localFile.Object);
                    task.Start();
                    return task;
                });

            var downloadRemoteItem = new Mock<IDownloadRemoteItem>();
            downloadRemoteItem.SetupGet(s => s.Item).Returns(remoteFile.Object);
            downloadRemoteItem.SetupGet(s => s.Destination).Returns(localDirectory.Object);

            var newDownloadRemoteItem = new Mock<IDownloadRemoteItem>();

            var resolutionService = new Mock<IResolve>();
            resolutionService.Setup(s => s.Resolve<IDownloadRemoteItem>())
            .Returns(newDownloadRemoteItem.Object);

            ftpServer.Setup(s => s.ConnectToServer())
                .Returns(() => ftpClient.Object);

            var sut = new DownloadDirectoryItems()
            {
                FtpServer = ftpServer.Object,
                Items = new List<IDownloadRemoteItem> { downloadRemoteItem.Object },
                DependencyService = resolutionService.Object
            };

            sut.Execute();

            localDirectory.Verify(v => v.TryGetItemAsync(It.Is<string>(i => i == name)));
            resolutionService.Verify(v => v.Resolve<IDownloadRemoteItem>());
            Assert.IsTrue(sut.FileConflicts.Any());
        }
    }
}
