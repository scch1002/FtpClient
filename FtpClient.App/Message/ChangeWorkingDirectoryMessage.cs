using FtpClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Message
{
    public class ChangeWorkingDirectoryMessage
    {
        public string Path { get; set; }
    }
}
