using Messenger.Core;
using Messenger.HelperClasses;
using Messenger.MVVM.View;
using Messenger.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.MVVM.Model
{
    public class MessageModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string FileName { get; set; }
        public string UsernameColor { get; set; }
        public string ImageSource { get; set;}
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public bool IsNativeOrigin { get; set; }
        public bool? FirstMessage { get; set; }

        private RelayCommand deleteCommand;
        private RelayCommand editCommand;
        private RelayCommand downloadCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? new RelayCommand(obj =>
                {
                    TcpClientWrapper.DeleteMessage(Id);
                });
            }
        }

        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ?? new RelayCommand(obj =>
                {
                    EditMessage editMenu = new EditMessage(Id, Message);
                    editMenu.Show();
                });
            }
        }

        public RelayCommand DownloadCommand
        {
            get
            {
                return downloadCommand ?? new RelayCommand(obj =>
                {
                    TcpClientWrapper.DownloadFile(Id);
                });
            }
        }
    }
}
