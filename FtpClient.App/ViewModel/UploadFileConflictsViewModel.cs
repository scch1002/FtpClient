using FtpClient.Message;
using FtpClient.Model;
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
    public class UploadFileConflictsViewModel : ViewModelBase 
    {
        public UploadFileConflictsViewModel()
        {
            ResolveConflictsCommand = new RelayCommand(ResolveConflicts);
            CancelConflictResolutionCommand = new RelayCommand(CancelConflictResolution);
        }
        
        public ICommand ResolveConflictsCommand { get; set; }

        public ICommand CancelConflictResolutionCommand { get; set; }

        public List<UploadFileConflictViewModel> Conflicts { get; set; } = new List<UploadFileConflictViewModel>();

        private void ResolveConflicts()
        {
            MessengerInstance.Send(new ResolveUploadConflictsMessage
            {
                ResolveConflicts = true,
                LocalItems = Conflicts.Where(w => w.OverWrite).Select(s => s.StorageItem).ToList()
            });
        }

        private void CancelConflictResolution()
        {
            MessengerInstance.Send(new ChangePageMessage());
        }
    }
}
