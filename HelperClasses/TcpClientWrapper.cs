using Messenger.MVVM.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Messenger.HelperClasses
{
    public static class TcpClientWrapper
    {
        private static TcpClient client;
        private static StreamWriter writer;
        private static StreamReader reader;
        private static string sessionId;

        public static event EventHandler<string> DataReceived;

        public static void Connect(string serverIP, int serverPort)
        {
            try
            {
                client = new TcpClient(serverIP, serverPort);
                writer = new StreamWriter(client.GetStream());
                reader = new StreamReader(client.GetStream());
                Console.WriteLine("Connected to server");

                BeginReceiveData();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to server: " + ex.Message);
            }
        }

        private static async void BeginReceiveData()
        {
            try
            {
                while (client != null && client.Connected)
                {
                    Trace.WriteLine("test");

                    string data = await reader.ReadLineAsync();
                    Trace.WriteLine("Received data from server: " + data);

                    DataReceived?.Invoke(null, data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error receiving data: " + ex.Message);
            }
        }

        private static void SendMessage(object data)
        {
            try
            {
                if (client == null || !client.Connected)
                {
                    Console.WriteLine("Error: Client is not connected");
                    return;
                }

                // Преобразуем объект в JSON и отправляем на сервер
                string jsonData = JsonConvert.SerializeObject(data);
                writer.WriteLine(jsonData);
                writer.Flush();
                Console.WriteLine("Data sent to server");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending message: " + ex.Message);
            }
        }

        public static void SendMessageToUser(string text, string receiver_user_id, string file_name, string file_content)
        {
            var data = new
            {
                text = text,
                receiverUserId = receiver_user_id,
                fileName = file_name,
                fileContent = file_content
            };

            var request = new
            {
                requestId = "Test",
                command = "sendMessage",
                data = data,
                sessionId = sessionId
            };

            SendMessage(request);
        }

        public static void SendMessageToChannel(string text, string receiver_channel_id, string file_name, string file_content)
        {
            var data = new
            {
                text = text,
                receiverChannelId = receiver_channel_id,
                fileName = file_name,
                fileContent = file_content
            };

            var request = new
            {
                requestId = "Test",
                command = "sendMessage",
                data = data,
                sessionId = sessionId
            };

            SendMessage(request);
        }

        public static void SetSessionId(string sessionKey)
        {
            sessionId = sessionKey;
        }

        public static void Authenticate(string email, string password)
        {
            var data = new
            {
                email = email,
                password = password
            };

            var request = new
            {
                requestId = "Test",
                command = "authenticate",
                data = data
            };

            SendMessage(request);
        }

        public static void AddNewUser(string name, string email, string role)
        {
            var data = new
            {
                name = name,
                email = email,
                role = role
            };

            var request = new
            {
                requestId = "Test",
                command = "addUser",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }

        public static void AddNewChannel(string name, string tag)
        {
            var data = new
            {
                name = name,
                tag = tag
            };

            var request = new
            {
                requestId = "Test",
                command = "addChannel",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }

        public static void Disconnect()
        {
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }

            if (client != null)
            {
                client.Close();
                client.Dispose();
            }

            Console.WriteLine("Client disconnect");
        }

        public static void LoadUsers()
        {
            var request = new
            {
                requestId = "Test",
                command = "getUsers",
                sessionId = sessionId

            };

            SendMessage(request);
        }

        public static void LoadChannels()
        {
            var request = new
            {
                requestId = "Test",
                command = "getChannels",
                sessionId = sessionId

            };

            SendMessage(request);
        }
        
        public static void LoadMyData()
        {
            var request = new
            {
                requestId = "Test",
                command = "getMyData",
                sessionId = sessionId
            };

            SendMessage(request);
        }

        public static void LoadDirectMessages(string interlocutorId)
        {
            var data = new
            {
                interlocutorId = interlocutorId
            };

            var request = new
            {
                requestId = "Test",
                command = "getDirectMessages",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }

        public static void LoadChannelMessages(string channelId)
        {
            var data = new
            {
                channelId = channelId
            };

            var request = new
            {
                requestId = "Test",
                command = "getChannelMessages",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }

        public static void SearchChannels(string tag)
        {
            var data = new
            {
                tag = tag
            };

            var request = new
            {
                requestId = "Test",
                command = "searchChannel",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }

        public static void DeleteUser(string userId)
        {
            var data = new
            {
                id = userId
            };

            var request = new
            {
                requestId = "Test",
                command = "deleteUser",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }

        public static void DeleteChannel(string channelId)
        {
            var data = new
            {
                id = channelId
            };

            var request = new
            {
                requestId = "Test",
                command = "deleteChannel",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }

        public static void BlockUser(string userId)
        {
            var data = new
            {
                id = userId
            };

            var request = new
            {
                requestId = "Test",
                command = "blockUser",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }

        public static void ActivateUser(string userId)
        {
            var data = new
            {
                id = userId
            };

            var request = new
            {
                requestId = "Test",
                command = "activateUser",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }

        public static void DeleteMessage(string messageId)
        {
            var data = new
            {
                id = messageId
            };

            var request = new
            {
                requestId = "Test",
                command = "deleteMessage",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }

        public static void EditMessage(string messageId, string text)
        {
            var data = new
            {
                id = messageId,
                text = text
            };

            var request = new
            {
                requestId = "Test",
                command = "editMessage",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }

        public static void DownloadFile(string messageId)
        {
            var data = new
            {
                messageId = messageId
            };

            var request = new
            {
                requestId = "Test",
                command = "downloadFile",
                sessionId = sessionId,
                data = data
            };

            SendMessage(request);
        }
    }
}