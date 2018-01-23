using FtpClient.Core.Model.FtpAction.Interface;
using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Model.FtpAction
{
    public class ChangeWorkingDirectory : FtpActionBase, IChangeWorkingDirectory
    {
        public string NewWorkingDirectoryPath { get; set; }

        protected override void FileAction(IFtpClient client)
        {
            client.SetWorkingDirectory(NewWorkingDirectoryPath);
        }
    }
}
