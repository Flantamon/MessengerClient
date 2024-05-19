using Messenger.Core;
using Messenger.HelperClasses;
using Messenger.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Messenger.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        public ObservableCollection<ChannelModel> Channels { get; set; }

        /*Commands*/
        public RelayCommand SendCommand { get; set; }

        private ContactModel _selectedContact;
        private ChannelModel _selectedChannel;
        public ContactModel SelectedContact
        {
            get => _selectedContact;
            set 
            { 
                _selectedContact = value;
                OnPropertyChanged();
            }
        }
        public ChannelModel SelectedChannel
        {
            get => _selectedChannel;
            set
            {
                _selectedChannel = value;
                OnPropertyChanged();
            }
        }

        private bool _showContacts = true;
        private bool _showChannels = false;
        public bool ShowContacts
        {
            get => _showContacts;
            set
            {
                _showContacts = value;
                OnPropertyChanged(nameof(ShowContacts));
            }
        }
        public bool ShowChannels
        {
            get => _showChannels;
            set
            {
                _showChannels = value;
                OnPropertyChanged(nameof(ShowChannels));
            }
        }

        private bool _isSearchVisible;
        public bool IsSearchVisible
        {
            get => _isSearchVisible;
            set
            {
                _isSearchVisible = value;
                OnPropertyChanged(nameof(IsSearchVisible));
            }
        }

        private string searchText = "";
        public string SearchText
        {
            get => searchText;
            set
            {
                if (SetProperty(ref searchText, value))
                {
                    OnPropertyChanged();
                }
            }
        }


        private Brush _contactsButtonForeground = Brushes.Gray;
        public Brush ContactsButtonForeground
        {
            get { return _contactsButtonForeground; }
            set
            {
                _contactsButtonForeground = value;
                OnPropertyChanged(nameof(ContactsButtonForeground));
            }
        }

        private Brush _channelsButtonForeground = Brushes.Gray;
        public Brush ChannelsButtonForeground
        {
            get { return _channelsButtonForeground; }
            set
            {
                _channelsButtonForeground = value;
                OnPropertyChanged(nameof(ChannelsButtonForeground));
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set 
            { 
                _message = value;
                OnPropertyChanged();
            }
        }

        private string _myName;
        public string MyName 
        { 
            get => _myName; 
            set
            {
                _myName = value;
                OnPropertyChanged();
            } 
        }

        private string _myId;
        public string MyId
        {
            get => _myId;
            set
            {
                _myId = value;
                OnPropertyChanged();
            }
        }

        private string _myRole;
        public string MyRole
        {
            get => _myRole;
            set
            {
                _myRole = value;
                OnPropertyChanged();
            }
        }

        private bool _isAdmin;
        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged();
            }
        }

        private string _myEmail;
        public string MyEmail 
        { 
            get => _myEmail; 
            set 
            { 
                _myEmail = value;
                OnPropertyChanged();
            } 
        }

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
            }
        }

        public MainViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();
            Channels = new ObservableCollection<ChannelModel>();

            SendCommand = new RelayCommand(o =>
            {
                string fileContent = null;
                string fileName = null;
                if (FilePath != null)
                {
                    byte[] fileBytes = File.ReadAllBytes(FilePath);
                    fileContent = Convert.ToBase64String(fileBytes);
                    fileName = Path.GetFileName(FilePath);
                }

                if (ShowContacts && SelectedContact != null)
                {
                    TcpClientWrapper.SendMessageToUser(Message, SelectedContact.Id, fileName, fileContent);
                }

                if (ShowChannels && SelectedChannel != null)
                {
                    TcpClientWrapper.SendMessageToChannel(Message, SelectedChannel.Id, fileName, fileContent);
                }

                Message = "";
                FilePath = null;
            });
        }

        private RelayCommand searchChannel;
        public ICommand SearchChannel => searchChannel ??= new RelayCommand(PerformSearchChannel);

        private void PerformSearchChannel(object commandParameter)
        {
            if (SearchText == "")
            {
                TcpClientWrapper.LoadChannels();
            }
            else
            {
                TcpClientWrapper.SearchChannels(SearchText);
            }
        }
    }
}
