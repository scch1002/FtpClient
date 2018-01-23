using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using FtpClient.Core.Model.Interface;
using FtpClient.Core.Model;
using FtpClient.Core;
using FtpClient.Core.Model.FtpAction.Interface;

namespace FtpClient.Model.FtpAction
{
    public class DownloadDirectoryItems : FtpActionBase, IDownloadDirectoryItems
    {
        public IResolve DependencyService { get; set; }

        public ICollection<IDownloadRemoteItem> Items { get; set; }

        public List<IDownloadRemoteItem> FileConflicts { get; } = new List<IDownloadRemoteItem>();

        protected override void FileAction(IFtpClient client)
        {
            foreach (var item in Items)
            {
                if (item.Item.Type == FileSystemItemType.File)
                {
                    DownloadFile(client, item.Destination, item.Item.FullName, item.Item, item.OverWrite);
                }
                else
                {
                    var newFolder = item.Destination.CreateFolderAsync(item.Item.Name).Result;
                    DownloadDirectory(client, newFolder, item.Item.FullName).Wait();
                }
            }
        }

        private void DownloadFile(IFtpClient client, ILocalDirectory storageFolder, string fullRemotePath, IRemoteItem item, bool overwrite)
        {
            var file = storageFolder.TryGetItemAsync(item.Name).Result;
            if (file != null && !overwrite)
            {
                var downloadRemoteItem = DependencyService.Resolve<IDownloadRemoteItem>();
                downloadRemoteItem.Destination = storageFolder;
                downloadRemoteItem.Item = item;

                FileConflicts.Add(downloadRemoteItem);
                return;
            }

            storageFolder.CreateFileAsync(item.Name, overwrite)
                .ContinueWith(fileStreamTask =>
                {
                    var stream = fileStreamTask.Result.OpenStreamForWriteAsync().Result;

                    using (var fileStream = stream)
                    {
                        client.Download(fileStream, fullRemotePath);
                    }
                }).Wait();
        }

        private Task DownloadDirectory(IFtpClient client, ILocalDirectory storageFolder, string remoteDirectoryPath)
        {
            return Task.Run(() =>
            {
                var files = client.GetItems(remoteDirectoryPath);

                if (files.Any())
                {
                    foreach (var file in files.Where(w => w.Type == FileSystemItemType.File))
                    {
                        DownloadFile(client, storageFolder, file.FullName, file, false);
                    }
                    foreach (var folder in files.Where(w => w.Type == FileSystemItemType.Directory))
                    {
                        var newFolder = storageFolder.CreateFolderAsync(folder.Name).Result;
                        DownloadDirectory(client, newFolder, folder.FullName).Wait();
                    }
                }
                return files;
            });
        }
    }
}
