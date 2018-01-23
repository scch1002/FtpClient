using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.ViewModel
{
    public class ChangePermissionsViewModel : ViewModelBase
    {
        public IDirectoryItemViewModel Item { get; set; }
    }
}
