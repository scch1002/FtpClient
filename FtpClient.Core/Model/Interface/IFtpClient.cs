using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model.Interface
{
    public interface IFtpClient : IDisposable
    {
        void Connect();
        void Disconnect();
        IRemoteItem[] GetItems(string Path);
        void SetWorkingDirectory(string Path);
        void DeleteFile(string Path);
        void DeleteDirectory(string Path);
        void Download(Stream fileStream, string fullRemotePath);
        void CreateDirectory(string newDirectoryPath);
        bool FileExists(string path);
        void Upload(Stream fileStream, string remotePath, bool overwrite);
    }
}
