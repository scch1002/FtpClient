using FluentFTP;
using FtpClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Message
{
    public class ChangePermissionsMessage
    {
        public string Path { get; set; }

        public FtpPermission Owner { get; set; }

        public FtpPermission Group { get; set; }

        public FtpPermission Other { get; set; }
    }
}
