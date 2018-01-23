using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.ViewModel.Interface
{
    public interface ILocalWorkingDirectoryViewModel
    {
        ILocalDirectory WorkingDirectory { get; set; }
    }
}
