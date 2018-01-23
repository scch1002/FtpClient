using FluentFTP;
using FtpClient.Core.Model.Interface;
using FtpClient.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using FtpClient.Core.Model;
using FtpClient.Core;

namespace FtpClient.Service
{
    public static class FtpService
    {
        public static List<ILocalDirectoryItemViewModel> CreatDirectoryItemViewModels(ILocalDirectory localDirectory)
        {
            var directoryItems = new List<ILocalDirectoryItemViewModel>();

            foreach(var file in localDirectory.GetFilesAsync().Result)
            {
                directoryItems.Add(CreateFileViewModel(file));
            }

            foreach(var folder in localDirectory.GetFoldersAsync().Result)
            {
                directoryItems.Add(CreateFileViewModel(folder));
            }

            return directoryItems;
        }

        private static LocalDirectoryViewModel CreateDirectoryViewModel(ILocalItem storageItem)
        {
            return new LocalDirectoryViewModel
            {
                StorageItem = storageItem
            };
        }

        private static LocalFileViewModel CreateFileViewModel(ILocalItem storageItem)
        {
            return new LocalFileViewModel
            {               
                StorageItem = storageItem,
            };
        }

        public static List<IRemoteDirectoryItemViewModel> CreatDirectoryItemViewModels(IEnumerable<IRemoteItem> ftpListItems)
        {
            var directoryItems = new List<IRemoteDirectoryItemViewModel>();

            foreach(var listItem in ftpListItems)
            {
                if (listItem.Type == FileSystemItemType.Directory)
                {
                    directoryItems.Add(CreateDirectoryViewModel(listItem));
                }
                else if (listItem.Type == FileSystemItemType.File)
                {
                    directoryItems.Add(CreateFileViewModel(listItem));
                }
            }

            return directoryItems;
        }

        private static RemoteDirectoryViewModel CreateDirectoryViewModel(IRemoteItem ftpListItem)
        {
            var directoryItems = new List<IDirectoryItemViewModel>();

            return new RemoteDirectoryViewModel
            {
                Item = ftpListItem
            };
        }

        private static RemoteFileViewModel CreateFileViewModel(IRemoteItem listItem)
        {
            return new RemoteFileViewModel
            {
                Item = listItem
            };
        }
    }
}
