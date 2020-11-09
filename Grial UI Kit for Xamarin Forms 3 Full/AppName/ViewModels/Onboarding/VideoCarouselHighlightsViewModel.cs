using System;
using System.Collections.ObjectModel;
using System.Globalization;
using AppName.Core;

namespace AppName
{
    public class VideoCarouselHighlightsViewModel : ObservableObject
    {
        public VideoCarouselHighlightsViewModel()
            : base(listenCultureChanges: true)
        {
            LoadData();
        }

        public string BackgroundVideo { get; set; }

        public ObservableCollection<HighlightData> Highlights { get; } = new ObservableCollection<HighlightData>();

        protected override void OnCultureChanged(CultureInfo culture)
        {
            LoadData();
        }

        private void LoadData()
        {
            Highlights.Clear();

            JsonHelper.Instance.LoadViewModel(this, source: "Onboarding.json");
        }
    }
}
