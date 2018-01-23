using FtpClient.Core;
using FtpClient.Core.Model.Interface;
using FtpClient.Core.Model;
using FtpClient.Model;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FtpRapper = FtpClient.Core.Model.FtpClient;
using FtpClient.Core.Model.FtpAction.Interface;
using FtpClient.Model.FtpAction;
using FtpClient.ViewModel.Interface;
using FtpClient.ViewModel;

namespace FtpClient.Service
{
    public class DependencyService : IResolve
    {
        private readonly UnityContainer _container;

        public DependencyService()
        {
            _container = new UnityContainer();
            Register();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        private void Register()
        {
            _container.RegisterInstance<IResolve>(this);
            _container.RegisterType<IFtpClient, FtpRapper>();
            _container.RegisterType<IFtpServer, FtpServer>();
            _container.RegisterType<ILocalDirectory, LocalDirectory>();
            _container.RegisterType<ILocalFile, LocalFile>();
            _container.RegisterType<IRemoteDirectory, RemoteDirectory>();
            _container.RegisterType<IRemoteFile, RemoteFile>();
            _container.RegisterType<IUploadLocalItem, UploadLocalItem>();
            _container.RegisterType<IDownloadRemoteItem, DownloadRemoteItem>();

            _container.RegisterType<IChangeWorkingDirectory, ChangeWorkingDirectory>();
            _container.RegisterType<IDeleteDirectoryItems, DeleteDirectoryItems>();
            _container.RegisterType<IDownloadDirectoryItems, DownloadDirectoryItems>();
            _container.RegisterType<IRetrieveDirectory, RetrieveDirectory>();
            _container.RegisterType<IUploadDirectoryItems, UploadDirectoryItems>();

            _container.RegisterType<IFtpServerViewModel, FtpServerViewModel>();
            _container.RegisterType<ILocalSystemViewModel, LocalSystemViewModel>();
            _container.RegisterType<IRemoteWorkingDirectoryViewModel, RemoteWorkingDirectoryViewModel>();
            _container.RegisterType<MainViewModel, MainViewModel>();
        }
    }
}
