using System.Collections.ObjectModel;
using System.Globalization;
using AppName.Core;

namespace AppName
{
    public class ChatMessagesListViewModel : ObservableObject
    {
        private readonly string _variantPageName;

        public ChatMessagesListViewModel(string variantPageName = null)
            : base(listenCultureChanges: true)
        {
            _variantPageName = variantPageName;

            LoadData();
        }

        public ObservableCollection<ChatMessageData> Messages { get; } = new ObservableCollection<ChatMessageData>();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Messages.Clear();

            JsonHelper.Instance.LoadViewModel(this, pageName: _variantPageName, source: "Messages.json");
        }
    }
}
