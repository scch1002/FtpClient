using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model.Interface
{
    public interface ILocalItem
    {
        string Name { get; }
        string FullName { get; }
        FileSystemItemType Type { get; }
    }
}
