using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FtpClient.ViewModel.Interface
{
    public interface ILocalSystemViewModel
    {
        ILocalWorkingDirectoryViewModel LocalWorkingDirectory { get; set; }

        ICommand SetLocalRootDirectoryCommand { get; set; }
    }
}
