using AppName.Core;
namespace AppName
{
    public class ChatMessageData
    {
        public ChatUserData From { get; set; }
        public string Body { get; set; }
        public string When { get; set; }
        public bool IsRead { get; set; }
        public bool IsInbound { get; set; }
    }

    public class ChatUserData
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
    }

    public class MessageData
    {
        public ChatUserData From { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool HasAttachment { get; set; }
        public int ThreadCount { get; set; }
        public string When { get; set; }
        public bool IsStared { get; set; }
        public bool IsRead { get; set; }
    }

    public class NotificationData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationType Type { get; set; }
    }
}
