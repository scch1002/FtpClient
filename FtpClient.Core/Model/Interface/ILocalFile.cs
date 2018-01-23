using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model.Interface
{
    public interface ILocalFile : ILocalItem
    {
        Task<Stream> OpenStreamForReadAsync();
        Task<Stream> OpenStreamForWriteAsync();
    }
}
