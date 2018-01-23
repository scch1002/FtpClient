using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FtpClient.Core.Model
{
    public class FtpClient : IFtpClient, IDependencyServiceProperty
    {
        private readonly FluentFTP.FtpClient _client;

        public FtpClient(FluentFTP.FtpClient client)
        {
            _client = client;
        }

        public IResolve DependencyService { get; set; }

        public void Connect()
        {
            _client.Connect();
        }

        public void CreateDirectory(string newDirectoryPath)
        {
            _client.CreateDirectory(newDirectoryPath);
        }

        public void DeleteDirectory(string Path)
        {
            _client.DeleteDirectory(Path);
        }

        public void DeleteFile(string Path)
        {
            _client.DeleteFile(Path);
        }

        public void Disconnect()
        {
            _client.Disconnect();
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public void Download(Stream fileStream, string fullRemotePath)
        {
            _client.Download(fileStream, fullRemotePath);
        }

        public bool FileExists(string path)
        {
            return _client.FileExists(path);
        }

        //TechDebt: Change interface to ICollection
        public IRemoteItem[] GetItems(string Path)
        {
            var ftpListItems = _client.GetListing(Path);
            var remoteItems = new List<IRemoteItem>();

            foreach(var item in ftpListItems)
            {
                IRemoteItem remoteItem = null;
                if (item.Type == FluentFTP.FtpFileSystemObjectType.File)
                {
                    var remoteFile = DependencyService.Resolve<IRemoteFile>();

                    remoteFile.FullName = item.FullName;
                    remoteFile.Name = item.Name;

                    remoteItem = remoteFile;
                }
                else if (item.Type == FluentFTP.FtpFileSystemObjectType.Directory)
                {
                    var remoteDirectory = DependencyService.Resolve<IRemoteDirectory>();

                    remoteDirectory.FullName = item.FullName;
                    remoteDirectory.Name = item.Name;

                    remoteItem = remoteDirectory;
                }
                else
                {
                    continue;
                }

                remoteItems.Add(remoteItem);
            }

            return remoteItems.ToArray();
        }

        public void SetWorkingDirectory(string Path)
        {
            _client.SetWorkingDirectory(Path);
        }

        public void Upload(Stream fileStream, string remotePath, bool overwrite)
        {
            _client.Upload(fileStream, remotePath, overwrite ? FluentFTP.FtpExists.Overwrite : FluentFTP.FtpExists.Skip);
        }
    }
}
