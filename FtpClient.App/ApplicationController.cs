using FtpClient.Core;
using FtpClient.Message;
using FtpClient.Service;
using FtpClient.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace FtpClient
{
    public class ApplicationController
    {
        public static IResolve DependencyService { get; set; }

        public ApplicationController()
        {
            DependencyService = new DependencyService();
            Messenger.Default.Register<ChangePageMessage>(this, ChangePage);
        }

        public MainViewModel MainViewModel { get; set; }

        public Frame AppFrame { get; set; }

        private async void ChangePage(ChangePageMessage message)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {   
                    if (message.ViewModel == null)
                    { 
                        AppFrame.GoBack();
                        return;
                    }
                    
                    if (message.ViewModel is MainViewModel)
                    {
                        AppFrame.Navigate(typeof(MainPage));
                        ((Page)AppFrame.Content).DataContext = message.ViewModel;
                    }
                    else if (message.ViewModel is UploadFileConflictsViewModel || message.ViewModel is DownloadFileConflictsViewModel)
                    {
                        AppFrame.Navigate(typeof(ResolveConflicts));
                        ((Page)AppFrame.Content).DataContext = message.ViewModel;
                    }
                });
        }
    }
}
