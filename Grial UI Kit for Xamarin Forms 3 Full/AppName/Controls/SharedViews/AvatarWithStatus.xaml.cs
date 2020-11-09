using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public enum AvatarStatus
    {
        Offline = 0,
        Online,
        Away,
        Busy
    }

    public partial class AvatarWithStatus : ContentView
    {
        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(
                nameof(Source),
                typeof(ImageSource),
                typeof(AvatarWithStatus),
                defaultValue: null);

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly BindableProperty StatusProperty =
            BindableProperty.Create(
                nameof(Status),
                typeof(string),
                typeof(AvatarWithStatus),
                defaultValue: default(AvatarStatus).ToString());

        public string Status
        {
            get { return (string)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public AvatarWithStatus()
        {
            InitializeComponent();
        }

        // Convenience property
        public AvatarStatus StatusEnum
        {
            get
            {
                return Enum.TryParse(Status, out AvatarStatus result) ?
                    result :
                    default(AvatarStatus);
            }

            set { Status = value.ToString(); }
        }
    }
}
