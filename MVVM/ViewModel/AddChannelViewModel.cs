using Messenger.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.MVVM.ViewModel
{
    public class AddChannelViewModel : ObservableObject
    {
        private string _channelName;
        public string ChannelName
        {
            get => _channelName;
            set
            {
                _channelName = value;
                OnPropertyChanged();
            }
        }

        private string _tag;
        public string Tag
        {
            get => _tag;
            set
            {
                _tag = value;
                OnPropertyChanged();
            }
        }
    }
}
