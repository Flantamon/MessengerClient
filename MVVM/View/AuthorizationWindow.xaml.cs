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
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private AuthorizationViewModel ViewModel { get; set; }
        public AuthorizationWindow()
        {
            InitializeComponent();

            ViewModel = new AuthorizationViewModel();

            DataContext = ViewModel;

            TcpClientWrapper.DataReceived += OnAuthenticationResponse;
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
            Application.Current.Shutdown();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)FindName("Password");

            TcpClientWrapper.Authenticate(ViewModel.Email, passwordBox.Password);
        }

        private void OnAuthenticationResponse(object sender, string rawResponse) 
        {
            try
            {
                var response = JsonConvert.DeserializeObject<AuthenticateResponse>(rawResponse);

                if (response.command == "authenticate")
                {
                    if (response.error == null)
                    {
                        TcpClientWrapper.SetSessionId(response.data.sessionId);

                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не существует либо заблокирован.");
                    }
                }
            } 
            catch (Exception ex)
            {

            }
        }
    }
}
