using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model.Interface
{
    public interface ILocalDirectory : ILocalItem
    {
        Task<ICollection<ILocalFile>> GetFilesAsync();
        Task<ICollection<ILocalDirectory>>  GetFoldersAsync();
        Task<ILocalDirectory> CreateFolderAsync(string name);
        Task<ILocalItem> TryGetItemAsync(string name);
        Task<ILocalFile> CreateFileAsync(string name, bool overWrite);
    }
}
