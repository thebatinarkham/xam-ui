using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class Badge : ContentView
    {
        public static readonly BindableProperty AutoHideProperty =
            BindableProperty.Create(
                nameof(AutoHide),
                typeof(bool),
                typeof(Badge),
                defaultValue: true,
                propertyChanged: (bindable, oldValue, newValue) => ((Badge)bindable).UpdateVisibility());

        public bool AutoHide
        {
            get { return (bool)GetValue(AutoHideProperty); }
            set { SetValue(AutoHideProperty, value); }
        }

        public static readonly new BindableProperty BackgroundColorProperty =
            BindableProperty.Create(
                nameof(BackgroundColor),
                typeof(Color),
                typeof(Badge),
                defaultValue: Color.Default,
                propertyChanged: (bindable, oldValue, newValue) => ((Badge)bindable).BadgeShape.BackgroundColor = (Color)newValue);

        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(
                nameof(TextColor),
                typeof(Color),
                typeof(Badge),
                defaultValue: Color.Default,
                propertyChanged: (bindable, oldValue, newValue) => ((Badge)bindable).LabelText.TextColor = (Color)newValue);

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(Badge),
                defaultValue: string.Empty,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var badge = (Badge)bindable;
                    badge.LabelText.Text = (string)newValue;
                    badge.UpdateVisibility();
                });

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Badge()
        {
            InitializeComponent();
        }

        private void UpdateVisibility()
        {
            IsVisible = !AutoHide || 
                (!string.IsNullOrWhiteSpace(LabelText.Text) && LabelText.Text.Trim() != "0");
        }
    }
}