using System.Collections.Generic;
using System.Linq;
using AppName.Core;

namespace AppName
{
    public class ChatMessagesViewModel : ObservableObject
    {
        private readonly string _contactId;
        private FlowConversationData _conversation;

        public ChatMessagesViewModel(string contactId)
        {
            _contactId = contactId;

            LoadData();
        }

        public FlowConversationData Conversation
        {
            get { return _conversation; }
            set { SetProperty(ref _conversation, value); }
        }

        private void LoadData()
        {
            Conversation = null;

            JsonHelper.Instance.LoadViewModel(this, source: "ChatFlow.json");

            if (_contactId != null)
            {
                var main = new ChatMainViewModel();

                Conversation = main.Conversations.FirstOrDefault(c => c.From.Id == _contactId);
                if (Conversation == null)
                {
                    Conversation = new FlowConversationData
                    {
                        From = main.Contacts.FirstOrDefault(c => c.Id == _contactId)
                    };
                }
            }
        }
    }
}
