using Messenger.Core;
using Messenger.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.MVVM.Model
{
    public class ChannelModel
    {
        public string Id { get; set; }
        public string ChannelName { get; set; }
        public string ImageSource { get; set; }
        public string Tag { get; set; }
        public bool IsAdmin { get; set; }

        private RelayCommand deleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? new RelayCommand(obj =>
                {
                    TcpClientWrapper.DeleteChannel(Id);
                });
            }
        }
    }
}
