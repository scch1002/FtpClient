using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model.FtpAction.Interface
{
    public interface IChangeWorkingDirectory: IFtpAction
    {
        string NewWorkingDirectoryPath { get; set; }
    }
}
