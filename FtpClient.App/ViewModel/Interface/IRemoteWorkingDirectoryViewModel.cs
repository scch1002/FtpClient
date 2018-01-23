using FtpClient.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.ViewModel.Interface
{
    public interface IRemoteWorkingDirectoryViewModel
    {
        string ServerPath { get; set; }

        ObservableCollection<IRemoteDirectoryItemViewModel> Items { get; set; }
    }
}
