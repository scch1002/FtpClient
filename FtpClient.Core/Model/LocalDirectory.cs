using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FtpClient.Core.Model
{
    public class LocalDirectory : ILocalDirectory
    {
        private readonly StorageFolder _storageFolder;

        public LocalDirectory(StorageFolder storageFolder)
        {
            _storageFolder = storageFolder;
        }

        public string Name => _storageFolder.Name;

        public string FullName => _storageFolder.Path;

        public FileSystemItemType Type => FileSystemItemType.Directory;

        public Task<ILocalFile> CreateFileAsync(string name, bool overWrite)
        {
            return _storageFolder.CreateFileAsync(name,
                    overWrite
                    ? CreationCollisionOption.ReplaceExisting
                    : CreationCollisionOption.FailIfExists)
                .AsTask()
                .ContinueWith(
                    storageFile => {
                        var localFile = new LocalFile(storageFile.Result);
                        return (ILocalFile)localFile;
                    });
        }

        public Task<ILocalDirectory> CreateFolderAsync(string name)
        {
            return _storageFolder.CreateFolderAsync(name)
                .AsTask()
                .ContinueWith(
                    storageFolder => {
                        var localDirectory = new LocalDirectory(storageFolder.Result);
                        return (ILocalDirectory)localDirectory;
                    });
        }

        public Task<ICollection<ILocalFile>> GetFilesAsync()
        {
            return _storageFolder.GetFilesAsync()
                .AsTask()
                .ContinueWith(
                    storageFiles => {
                        var items = storageFiles.Result;

                        var localFiles = new List<ILocalFile>();

                        foreach (var storageFile in items)
                        {
                            localFiles.Add(new LocalFile(storageFile));
                        }

                        return (ICollection<ILocalFile>)localFiles;
                    });
        }

        public Task<ICollection<ILocalDirectory>> GetFoldersAsync()
        {
            return _storageFolder.GetFoldersAsync()
                .AsTask()
                .ContinueWith(
                    storageFolders => {
                        var items = storageFolders.Result;

                        var localFiles = new List<ILocalDirectory>();

                        foreach (var storageFolder in items)
                        {
                            localFiles.Add(new LocalDirectory(storageFolder));
                        }

                        return (ICollection<ILocalDirectory>)localFiles;
                    });
        }

        public Task<ILocalItem> TryGetItemAsync(string name)
        {
            return _storageFolder.TryGetItemAsync(name)
                .AsTask()
                .ContinueWith(
                    storageItem => {
                        var item = storageItem.Result;

                        ILocalItem localItem = null;

                        if (item == null)
                        {
                            return localItem;
                        }

                        if (item.IsOfType(StorageItemTypes.File))
                        {
                            localItem = new LocalFile((StorageFile)item);
                        }

                        if (item.IsOfType(StorageItemTypes.Folder))
                        {
                            localItem = new LocalDirectory((StorageFolder)item);
                        }

                        return localItem;
                    });
        }
    }
}
