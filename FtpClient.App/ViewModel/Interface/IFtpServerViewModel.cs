using FtpClient.Core;
using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.ViewModel.Interface
{
    public interface IFtpServerViewModel: IDependencyServiceProperty
    {
        IFtpServer FtpServer { get; set; }

        IRemoteWorkingDirectoryViewModel RemoteWorkingDirectory { get; set; }
    }
}
