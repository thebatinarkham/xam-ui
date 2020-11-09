using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class SocialHeaderTemplate : ContentView
    {
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(
                nameof(Text),
                typeof(string),
                typeof(SocialHeaderTemplate),
                string.Empty,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (SocialHeaderTemplate)bindable;
                    ctrl.HeaderLabel.Text = (string)newValue;
                });

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /* ICON */

        public static readonly BindableProperty IconTextProperty =
            BindableProperty.Create(
                nameof(IconText),
                typeof(string),
                typeof(SocialHeaderTemplate),
                string.Empty,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (SocialHeaderTemplate)bindable;
                    ctrl.HeaderIcon.Text = (string)newValue;
                });

        public string IconText
        {
            get { return (string)GetValue(IconTextProperty); }
            set { SetValue(IconTextProperty, value); }
        }

        public SocialHeaderTemplate()
        {
            InitializeComponent();
        }
    }
}