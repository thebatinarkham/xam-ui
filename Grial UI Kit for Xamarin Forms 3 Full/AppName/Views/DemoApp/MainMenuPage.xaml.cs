using System;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage(Action<Page> openPageAsRoot)
        {
            InitializeComponent();

            BindingContext = new MainMenuViewModel(Navigation, openPageAsRoot);
        }
    }
}