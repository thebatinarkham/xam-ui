using System;
using AppName.Core;

namespace AppName
{
    public class FriendData
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
    }

    public class ProfileData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
    }

    public class TimelineEventData
    {
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
        public string Image { get; set; }
        public string When { get; set; }
    }
}
