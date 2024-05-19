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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private RegistrationViewModel ViewModel { get; set; }
        public RegistrationWindow()
        {
            InitializeComponent();

            ViewModel = new RegistrationViewModel();

            DataContext = ViewModel;

            TcpClientWrapper.DataReceived += OnAddUserResponse;
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

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            var roleAdmin = (RadioButton)FindName("Admin");

            string role = "user";

            if ((bool)roleAdmin.IsChecked)
            {
                role = "admin";
            }

            TcpClientWrapper.AddNewUser(ViewModel.Name, ViewModel.Email, role);
        }

        private void OnAddUserResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<AddUserResponse>(rawResponse);

                if (response.command == "addUser")
                {
                    if (response.error == null)
                    {
                        MessageBox.Show("Пользователь успешно добавлен!");

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
