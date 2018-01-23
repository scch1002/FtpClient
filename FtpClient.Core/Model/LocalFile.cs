using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;

namespace FtpClient.Core.Model
{
    public class LocalFile : ILocalFile
    {
        private readonly StorageFile _storageFile;

        public LocalFile(StorageFile storageFile)
        {
            _storageFile = storageFile;
        }

        public string Name => _storageFile.Name;

        public string FullName => _storageFile.Path;

        public FileSystemItemType Type => FileSystemItemType.File;

        public Task<Stream> OpenStreamForReadAsync()
        {
            return _storageFile.OpenStreamForReadAsync();
        }

        public Task<Stream> OpenStreamForWriteAsync()
        {
            return _storageFile.OpenStreamForWriteAsync();
        }
    }
}
