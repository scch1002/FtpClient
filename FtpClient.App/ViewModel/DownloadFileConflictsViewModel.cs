using FtpClient.Message;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FtpClient.ViewModel
{
    public class DownloadFileConflictsViewModel : ViewModelBase
    {
        public DownloadFileConflictsViewModel()
        {
            ResolveConflictsCommand = new RelayCommand(ResolveConflicts);
            CancelConflictResolutionCommand = new RelayCommand(CancelConflictResolution);
        }

        public ICommand ResolveConflictsCommand { get; set; }

        public ICommand CancelConflictResolutionCommand { get; set; }

        public List<DownloadFileConflictViewModel> Conflicts { get; set; } = new List<DownloadFileConflictViewModel>();

        private void ResolveConflicts()
        {
            MessengerInstance.Send(new ResolveDownloadConflictsMessage
            {
                ResolveConflicts = true,
                LocalItems = Conflicts.Where(w => w.OverWrite).Select(s => s.Item).ToList()
            });
        }

        private void CancelConflictResolution()
        {
            MessengerInstance.Send(new ChangePageMessage());
        }
    }
}
