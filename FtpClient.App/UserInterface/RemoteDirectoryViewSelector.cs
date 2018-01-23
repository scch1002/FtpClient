using FtpClient.Core.Model;
using FtpClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FtpClient.UserInterface
{
    public class RemoteDirectoryViewSelector : DataTemplateSelector
    {
        public DataTemplate FileTemplate { get; set; }

        public DataTemplate DirectoryTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var directoryItem = item as IDirectoryItemViewModel;
            if (directoryItem == null)
            {
                return null;
            }

            var element = container as FrameworkElement;
            if (element == null)
            {
                return null;
            }

            if (directoryItem.Type == FileSystemItemType.Directory)
            {
                return DirectoryTemplate;
            }
            else
            {
                return FileTemplate;
            }
        }
    }
}
