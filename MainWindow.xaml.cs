using Messenger.Core;
using Messenger.HelperClasses;
using Messenger.MVVM.DTO;
using Messenger.MVVM.Model;
using Messenger.MVVM.View;
using Messenger.MVVM.ViewModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Messenger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainViewModel();

            DataContext = ViewModel;

            TcpClientWrapper.DataReceived += OnLoadMyDataResponse;
            TcpClientWrapper.DataReceived += OnLoadChannelsResponse;
            TcpClientWrapper.DataReceived += OnLoadUsersResponse;
            TcpClientWrapper.DataReceived += OnLoadMessagesResponse;
            TcpClientWrapper.DataReceived += OnPullMessagesResponse;
            TcpClientWrapper.DataReceived += OnDeleteUserResponse;
            TcpClientWrapper.DataReceived += OnDeleteChannelResponse;
            TcpClientWrapper.DataReceived += OnBlockUserResponse;
            TcpClientWrapper.DataReceived += OnActivateUserResponse;
            TcpClientWrapper.DataReceived += OnSendMessageResponse;
            TcpClientWrapper.DataReceived += OnDeleteMessageResponse;
            TcpClientWrapper.DataReceived += OnEditMessageResponse;
            TcpClientWrapper.DataReceived += OnBlockedOrDeletedUserResponse;
            TcpClientWrapper.DataReceived += OnDownloadFileResponse;

            ViewModel.ChannelsButtonForeground = Brushes.Gray;
            ViewModel.ContactsButtonForeground = Brushes.Gray;

            TcpClientWrapper.LoadMyData();
            TcpClientWrapper.LoadUsers();
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

        private void WindowStateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowContacts_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowContacts = true;
            ViewModel.ShowChannels = false;

            ViewModel.ContactsButtonForeground = Brushes.White;
            ViewModel.ChannelsButtonForeground = Brushes.Gray;

            ViewModel.IsSearchVisible = false;

            TcpClientWrapper.LoadUsers();
        }

        private void ShowChannels_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ShowContacts = false;
            ViewModel.ShowChannels = true;

            ViewModel.ChannelsButtonForeground = Brushes.White;
            ViewModel.ContactsButtonForeground = Brushes.Gray;


            ViewModel.IsSearchVisible = true;

            TcpClientWrapper.LoadChannels();
        }

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registration = new RegistrationWindow();
            registration.Show();
        }

        private void AddChannelButton_Click(Object sender, RoutedEventArgs e)
        {
            AddChannelWindow addChannel = new AddChannelWindow();
            addChannel.Show();
        }

        private void Contacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ListView listView = sender as ListView;

            if (listView.SelectedItems.Count > 0)
            {
                ContactModel selectedItem = listView.SelectedItem as ContactModel;
                TcpClientWrapper.LoadDirectMessages(selectedItem.Id);
            }
        }

        private void Channels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ListView listView = sender as ListView;

            if (listView.SelectedItems.Count > 0)
            {
                ChannelModel selectedItem = listView.SelectedItem as ChannelModel;
                TcpClientWrapper.LoadChannelMessages(selectedItem.Id);
            }
        }

        private void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt|Image files (*.png;*.jpg)|*.png;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                ViewModel.FilePath = filePath;

                string fileName = Path.GetFileName(filePath);
                var fileNameLabel = (Label)FindName("FileName");
                fileNameLabel.Content = fileName;
            }
        }

        private void OnSendMessageResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<SendMessageResponse>(rawResponse);

                if (response.command == "sendMessage")
                {
                    if (response.error == null)
                    {
                        var fileNameLabel = (Label)FindName("FileName");
                        fileNameLabel.Content = "";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnLoadUsersResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<LoadUsersResponse>(rawResponse);

                if (response.command == "getUsers")
                {
                    if (response.error == null)
                    {
                        ViewModel.Contacts.Clear();
                        for (int i = 0; i < response.data.Length; i++)
                        {
                            ViewModel.Contacts.Add(new ContactModel
                            {
                                Id = response.data[i].id,
                                Username = response.data[i].name,
                                ImageSource = "https://i.imgur.com/G4FzuHHb.jpg",
                                Status = response.data[i].status,
                                IsAdmin = ViewModel.MyRole == "admin"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnPullMessagesResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<PullMessagesResponse>(rawResponse);

                if (response.command == "pullMessages")
                {
                    if (ViewModel.ShowContacts)
                    {
                        TcpClientWrapper.LoadDirectMessages(ViewModel.SelectedContact.Id);
                    }

                    if (ViewModel.ShowChannels)
                    {
                        TcpClientWrapper.LoadChannelMessages(ViewModel.SelectedChannel.Id);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnLoadMessagesResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<LoadMessagesResponse>(rawResponse);

                if (response.command == "getDirectMessages" || response.command == "getChannelMessages")
                {
                    if (response.error == null)
                    {
                        ViewModel.Messages.Clear();
                        for (int i = 0; i < response.data.Length; i++)
                        {

                            ViewModel.Messages.Add(new MessageModel
                            {
                                Id = response.data[i].id,
                                Username = response.data[i].sender_name,
                                FileName = response.data[i].file_name,
                                UsernameColor = "#409aff",
                                ImageSource = "https://i.imgur.com/yMWvLXd.png",
                                Message = response.data[i].text,
                                Time = TimeZoneInfo.ConvertTimeFromUtc(response.data[i].created_at, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time")),
                                FirstMessage = true
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnLoadChannelsResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<LoadChannelsResponse>(rawResponse);

                if (response.command == "getChannels" || response.command == "searchChannel")
                {
                    if (response.error == null)
                    {
                        ViewModel.Channels.Clear();
                        for (int i = 0; i < response.data.Length; i++)
                        {
                            ViewModel.Channels.Add(new ChannelModel
                            {
                                Id = response.data[i].id,
                                ChannelName = response.data[i].name,
                                ImageSource = "https://i.imgur.com/G4FzuHHb.jpg",
                                Tag = response.data[i].tag,
                                IsAdmin = ViewModel.MyRole == "admin"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnLoadMyDataResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<LoadMyDataResponse>(rawResponse);

                if (response.command == "getMyData")
                {
                    if (response.error == null)
                    {
                        ViewModel.MyId = response.data.id;
                        ViewModel.MyName = response.data.name;
                        ViewModel.MyRole = response.data.role;
                        ViewModel.MyEmail = response.data.email;
                        ViewModel.IsAdmin = response.data.role == "admin";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnDeleteUserResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<DeleteUserResponse>(rawResponse);

                if (response.command == "deleteUser")
                {
                    if (response.error == null)
                    {
                        MessageBox.Show("Пользователь успешно удален!");
                        TcpClientWrapper.LoadUsers();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnDeleteChannelResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<DeleteChannelResponse>(rawResponse);

                if (response.command == "deleteChannel")
                {
                    if (response.error == null)
                    {
                        MessageBox.Show("Канал успешно удален!");
                        TcpClientWrapper.LoadChannels();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnDeleteMessageResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<DeleteMessageResponse>(rawResponse);

                if (response.command == "deleteMessage")
                {
                    if (response.error == null)
                    {
                        if (ViewModel.ShowContacts)
                        {
                            TcpClientWrapper.LoadDirectMessages(ViewModel.SelectedContact.Id);
                        }

                        if (ViewModel.ShowChannels)
                        {
                            TcpClientWrapper.LoadChannelMessages(ViewModel.SelectedChannel.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnEditMessageResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<EditMessageResponse>(rawResponse);

                if (response.command == "editMessage")
                {
                    if (response.error == null)
                    {
                        if (ViewModel.ShowContacts)
                        {
                            TcpClientWrapper.LoadDirectMessages(ViewModel.SelectedContact.Id);
                        }

                        if (ViewModel.ShowChannels)
                        {
                            TcpClientWrapper.LoadChannelMessages(ViewModel.SelectedChannel.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnBlockUserResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<BlockUserResponse>(rawResponse);

                if (response.command == "blockUser")
                {
                    if (response.error == null)
                    {
                        MessageBox.Show("Пользователь успешно заблокирован!");
                        TcpClientWrapper.LoadUsers();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnActivateUserResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<ActivateUserResponse>(rawResponse);

                if (response.command == "activateUser")
                {
                    if (response.error == null)
                    {
                        MessageBox.Show("Пользователь успешно разблокирован!");
                        TcpClientWrapper.LoadUsers();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnBlockedOrDeletedUserResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<BlockedOrDeletedUserResponse>(rawResponse);

                if (response.command == "userBlocked" && response.data.userId == ViewModel.MyId)
                {
                    if (response.error == null)
                    {
                        MessageBox.Show("Вы были заблокированы администратором.");
                        Application.Current.Shutdown();
                    }
                }
                if (response.command == "userDeleted" && response.data.userId == ViewModel.MyId)
                {
                    if (response.error == null)
                    {
                        MessageBox.Show("Вы были удалены администратором.");
                        Application.Current.Shutdown();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnDownloadFileResponse(object sender, string rawResponse)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<DownloadFileResponse>(rawResponse);

                if (response.command == "downloadFile")
                {
                    if (response.error == null)
                    {
                        string downloadsPath = GetDownloadsPath();
                        string filePath = downloadsPath + "\\" + response.data.file_name; 

                        SaveBase64ToFile(response.data.file_content, filePath);
                        MessageBox.Show("Файл сохранен в папку <Загрузки>");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void SaveBase64ToFile(string base64String, string filePath)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64String);

                File.WriteAllBytes(filePath, bytes);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
            }
        }

        static string GetDownloadsPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        }
    }
}