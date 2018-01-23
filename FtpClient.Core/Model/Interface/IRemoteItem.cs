using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model.Interface
{
    public interface IRemoteItem
    {
        string Name { get; set; }
        string FullName { get; set; }
        FileSystemItemType Type { get; }
    }
}
