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
    public class UploadDirectoryItems : FtpActionBase, IUploadDirectoryItems
    {
        public UploadDirectoryItems(IResolve dependencyService)
        {
            DependencyService = dependencyService;
        }

        public IResolve DependencyService { get; set; }

        public ICollection<IUploadLocalItem> Items { get; set; } = new List<IUploadLocalItem>();

        public List<IUploadLocalItem> FileConflicts { get; } = new List<IUploadLocalItem>();

        protected override void FileAction(IFtpClient client)
        {
            foreach (var item in Items)
            {
                if (item.Item.Type == FileSystemItemType.File)
                {
                    UploadFile(client, (ILocalFile)item.Item, item.DestinationPath, item.OverWrite).Wait();
                }
                else
                {
                    var folder = (ILocalDirectory)item.Item;
                    var newDirectoryPath = item.DestinationPath + "/" + folder.Name;
                    client.CreateDirectory(newDirectoryPath);
                    UploadDirectory(client, folder, newDirectoryPath).Wait();
                }
            }
        }

        private Task UploadFile(IFtpClient client, ILocalFile file, string remoteDirectoryPath, bool overwrite)
        {
            return file.OpenStreamForReadAsync().ContinueWith(fileStreamTask =>
            {
                if(client.FileExists(remoteDirectoryPath + "/" + file.Name) && !overwrite)
                {
                    var localDirectoryItem = DependencyService.Resolve<IUploadLocalItem>();

                    localDirectoryItem.Item = file;
                    localDirectoryItem.DestinationPath = remoteDirectoryPath;

                    FileConflicts.Add(localDirectoryItem);
                    return;
                }

                using (var fileStream = fileStreamTask.Result)
                {
                    client.Upload(fileStream, remoteDirectoryPath + "/" + file.Name, overwrite);
                }
            });
        }

        private Task UploadDirectory(IFtpClient client, ILocalDirectory storagefolder, string remoteDirectoryPath)
        {
            return Task.Run(() =>
            {
                return storagefolder.GetFilesAsync().Result;
            })
            .ContinueWith(fileTask =>
            {
                var files = fileTask.Result;

                if (files != null && files.Any())
                {
                    foreach (var file in files)
                    {
                        UploadFile(client, file, remoteDirectoryPath, false).Wait();
                    }
                }
            }).ContinueWith(folderTask =>
            {
                return storagefolder.GetFoldersAsync().Result;
            })
            .ContinueWith(folderTask =>
            {
                var folders = folderTask.Result;

                if (folders == null || !folders.Any())
                {
                    return;
                }

                foreach (var folder in folders)
                {
                    var newDirectoryPath = remoteDirectoryPath + "/" + folder.Name;
                    client.CreateDirectory(newDirectoryPath);
                    UploadDirectory(client, folder, newDirectoryPath).Wait();
                }
            });
        }
    }
}
