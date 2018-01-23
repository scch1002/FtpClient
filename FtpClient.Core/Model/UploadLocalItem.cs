using FtpClient.Core.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core.Model
{
    public class UploadLocalItem : IUploadLocalItem
    {
        public ILocalItem Item { get; set; }

        public string DestinationPath { get; set; }

        public bool OverWrite { get; set; }
    }
}
