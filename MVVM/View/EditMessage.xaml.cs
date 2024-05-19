using Messenger.HelperClasses;
using Messenger.MVVM.DTO;
using Messenger.MVVM.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Messenger.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для EditMessage.xaml
    /// </summary>
    public partial class EditMessage : Window
    {
        private EditMessageViewModel ViewModel { get; set; }
        private string messageId;

        public EditMessage(string messageId, string text)
        {
            InitializeComponent();

            ViewModel = new EditMessageViewModel();
            ViewModel.Message = text;

            this.messageId = messageId;
            DataContext = ViewModel;
        }
        
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            TcpClientWrapper.EditMessage(messageId, ViewModel.Message);
            Close();
        }
    }
}

