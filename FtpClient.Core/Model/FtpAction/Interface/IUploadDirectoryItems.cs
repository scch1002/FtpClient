using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model.FtpAction.Interface
{
    public interface IUploadDirectoryItems: IFtpAction, IDependencyServiceProperty
    {
        ICollection<IUploadLocalItem> Items { get; set; }

        List<IUploadLocalItem> FileConflicts { get; }
    }
}
