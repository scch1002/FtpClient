using FtpClient.Core.Model;
using FtpClient.Core.Model.FtpAction.Interface;
using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Model.FtpAction
{
    public class DeleteDirectoryItems : FtpActionBase, IDeleteDirectoryItems
    {
        public ICollection<IRemoteItem> Items { get; set; }

        protected override void FileAction(IFtpClient client)
        {
            foreach (var item in Items)
            {
                if (item.Type == FileSystemItemType.File)
                {
                    DeleteFile(client, item.FullName).Wait();
                }
                else
                {
                    DeleteDirectory(client, item.FullName).Wait();
                }
            }
        }

        private static Task DeleteFile(IFtpClient client, string remoteFilePath)
        {
            return Task.Run(() =>
            {
                client.DeleteFile(remoteFilePath);
            });
        }

        private static Task DeleteDirectory(IFtpClient client, string remoteDirectoryPath)
        {
            return Task.Run(() =>
            {
                client.DeleteDirectory(remoteDirectoryPath);
            });
        }
    }
}
