using FtpClient.Core.Model.FtpAction.Interface;
using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Model.FtpAction
{
    public class RetrieveDirectory : FtpActionBase, IRetrieveDirectory
    {
        public string Path { get; set; }

        protected override void FileAction(IFtpClient client)
        {
            DirectoryItems = client.GetItems(Path);
        }

        public ICollection<IRemoteItem> DirectoryItems { get; set; }
    }
}
