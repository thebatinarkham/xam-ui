using AppName.Core;
namespace AppName
{
    public class FlowContactData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Organization { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Notes { get; set; }
        public string LastSeen { get; set; }
    }

    public class FlowConversationData
    {
        public FlowContactData From { get; set; }
        public FlowMessageData[] Messages { get; set; }
        public FlowMessageData Preview { get; set; }
    }

    public class FlowMessageData
    {
        public string When { get; set; }
        public string Text { get; set; }
        public bool IsRead { get; set; }
        public bool IsIncoming { get; set; }
    }
}
