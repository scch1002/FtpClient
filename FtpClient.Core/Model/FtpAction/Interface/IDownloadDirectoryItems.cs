using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model.FtpAction.Interface
{
    public interface IDownloadDirectoryItems : IFtpAction, IDependencyServiceProperty
    {
        ICollection<IDownloadRemoteItem> Items { get; set; }

        List<IDownloadRemoteItem> FileConflicts { get; }
    }
}
