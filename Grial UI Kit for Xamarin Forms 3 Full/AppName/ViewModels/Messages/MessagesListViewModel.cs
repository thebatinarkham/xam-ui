using System.Collections.ObjectModel;
using System.Globalization;
using AppName.Core;

namespace AppName
{
    public class MessagesListViewModel : ObservableObject
    {
        public MessagesListViewModel()
            : base(listenCultureChanges: true)
        {
            LoadData();
        }

        public ObservableCollection<MessageData> Messages { get; } = new ObservableCollection<MessageData>();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Messages.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "Messages.json");
        }
    }
}
