using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.MVVM.DTO
{
    class BaseResponse<T>
    {
        public string requestId;
        public string command;
        public T data;
        public int? error;
    }

    class User
    {
        public string id;
        public string name;
        public string email;
        public string role;
        public string status;
    }

    class Channel
    {
        public string id;
        public string name;
        public string tag;
    }

    class Message
    {
        public string id;
        public string sender_id;
        public string receiver_channel_id;
        public string receiver_user_id;
        public string sender_name;
        public DateTime created_at;
        public DateTime? updated_at;
        public string text;
        public string file_name;
    }

    class UserIdData
    {
        public string userId;
    }

    class FileData
    {
        public string id;
        public string file_name;
        public string file_content;
    }

    class AuthenticateData 
    {
        public string sessionId;
        
    }
    class AuthenticateResponse : BaseResponse<AuthenticateData> { }

    class AddUserResponse : BaseResponse<string> { }
    class AddChannelResponse : BaseResponse<string> { }

    class LoadUsersResponse : BaseResponse<User[]> { }

    class PullMessagesResponse 
    {
        public string command;
    }

    class DeleteUserResponse : BaseResponse<string> { }
    class DeleteChannelResponse : BaseResponse<string> { }

    class SendMessageResponse : BaseResponse<string> { }
    class DeleteMessageResponse : BaseResponse<string> { }
    class EditMessageResponse : BaseResponse<string> { }

    class DownloadFileResponse : BaseResponse<FileData> { }

    class BlockUserResponse : BaseResponse<string> { }
    class ActivateUserResponse : BaseResponse<string> { }
    class BlockedOrDeletedUserResponse : BaseResponse<UserIdData> { }


    class LoadChannelsResponse : BaseResponse<Channel[]> { }
    class LoadMessagesResponse : BaseResponse<Message[]> { }
    class LoadMyDataResponse : BaseResponse<User> { }
}
