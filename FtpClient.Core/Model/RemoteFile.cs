using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model
{
    public class RemoteFile : IRemoteFile
    {
        public string Name { get; set; }

        public string FullName { get; set; }

        public FileSystemItemType Type => FileSystemItemType.File;
    }
}
