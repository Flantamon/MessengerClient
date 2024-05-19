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
    /// Логика взаимодействия для AddChannelWindow.xaml
    /// </summary>
    public partial class AddChannelWindow : Window
    {
        private AddChannelViewModel ViewModel { get; set; }
        public AddChannelWindow()
        {
            InitializeComponent();

            ViewModel = new AddChannelViewModel();

            DataContext = ViewModel;

            TcpClientWrapper.DataReceived += OnAddChannelResponse;
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

        private void AddChannelButton_Click(object sender, RoutedEventArgs e)
        {
            TcpClientWrapper.AddNewChannel(ViewModel.ChannelName, ViewModel.Tag);
        }

        private void OnAddChannelResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<AddChannelResponse>(rawResponse);

                if (response.command == "addChannel")
                {
                    if (response.error == null)
                    {
                        MessageBox.Show("Канал успешно создан!");

                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
