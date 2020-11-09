using System.Collections.ObjectModel;
using AppName.Core;

namespace AppName
{
    public class ChatMainViewModel : ObservableObject
    {
        public ChatMainViewModel()
        {
            LoadData();
        }

        public ObservableCollection<FlowContactData> Contacts { get; } = new ObservableCollection<FlowContactData>();
        public ObservableCollection<FlowConversationData> Conversations { get; } = new ObservableCollection<FlowConversationData>();

        private void LoadData()
        {
            Contacts.Clear();
            Conversations.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "ChatFlow.json");
        }
    }
}
