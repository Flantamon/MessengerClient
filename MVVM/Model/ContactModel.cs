using Messenger.Core;
using Messenger.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Messenger.MVVM.Model
{
    public class ContactModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string ImageSource { get; set; }
        public string Status { get; set; }
        public bool IsAdmin { get; set; }


        private RelayCommand deleteCommand;
        private RelayCommand blockCommand;
        private RelayCommand activateCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? new RelayCommand(obj =>
                {
                    TcpClientWrapper.DeleteUser(Id);
                });
            }
        }

        public RelayCommand BlockCommand
        {
            get
            {
                return blockCommand ?? new RelayCommand(obj =>
                {
                    TcpClientWrapper.BlockUser(Id);
                });
            }
        }

        public RelayCommand ActivateCommand
        {
            get
            {
                return activateCommand ?? new RelayCommand(obj =>
                {
                    TcpClientWrapper.ActivateUser(Id);
                });
            }
        }
    }
}
