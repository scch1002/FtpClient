using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Model.FtpAction
{
    public abstract class FtpActionBase
    {
        public IFtpServer FtpServer { get; set; }

        public bool FailedToConnectToServer { get; set; }

        public void Execute()
        {
            IFtpClient client;
            try
            {
                client = FtpServer.ConnectToServer();
                client.Connect();
            }
            catch (Exception)
            {
                FailedToConnectToServer = true;
                return;
            }

            FileAction(client);

            client.Disconnect();
            client.Dispose();
        }

        protected abstract void FileAction(IFtpClient client);
    }
}
