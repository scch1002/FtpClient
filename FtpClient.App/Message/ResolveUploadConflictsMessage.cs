using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Message
{
    public class ResolveUploadConflictsMessage
    {
        public bool ResolveConflicts { get; set; }

        public ICollection<IUploadLocalItem> LocalItems { get; set; }
    }
}
