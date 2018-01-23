using FluentFTP;
using FtpClient.Core.Model.Interface;
using FtpClient.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using IFtpClient = FtpClient.Core.Model.Interface.IFtpClient;
using FtpRapper = FtpClient.Core.Model.FtpClient;

namespace FtpClient.Core.Model
{
    public class FtpServer : IFtpServer 
    {
        public FtpServer(IResolve dependencyService)
        {
            DependencyService = dependencyService;
        }

        public string ServerUrl { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public IResolve DependencyService { get; set; }

        public IFtpClient ConnectToServer()
        {
            FluentFTP.FtpClient client;
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                client = new FluentFTP.FtpClient(ServerUrl, GetCredential());
            }
            else
            {
                client = new FluentFTP.FtpClient(ServerUrl);
            }

            client.EncryptionMode = FtpEncryptionMode.None;
            client.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);

            return new FtpRapper(client)
            {
                DependencyService = DependencyService.Resolve<IResolve>()
            };
        }

        private void OnValidateCertificate(FluentFTP.FtpClient control, FtpSslValidationEventArgs e)
        {
            // add logic to test if certificate is valid here
            e.Accept = true;
        }

        private NetworkCredential GetCredential()
        {
            return new NetworkCredential(UserName, UserPassword);
        }
    }
}
