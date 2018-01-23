using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model.Interface
{
    public interface IFtpServer : IDependencyServiceProperty
    {
        string ServerUrl { get; set; }

        string UserName { get; set; }

        string UserPassword { get; set; }

        IFtpClient ConnectToServer();
    }
}
