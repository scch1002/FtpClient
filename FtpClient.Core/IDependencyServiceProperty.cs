using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpClient.Core
{
    public interface IDependencyServiceProperty
    {
        IResolve DependencyService { get; set; }
    }
}
