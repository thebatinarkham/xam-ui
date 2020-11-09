using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using AppName.Core;

namespace AppName
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly CultureChangeNotifier _notifier;

        public ObservableObject(bool listenCultureChanges = false)
        {
            if (listenCultureChanges)
            {
                _notifier = new CultureChangeNotifier();
                _notifier.CultureChanged += CultureChanged;
            }
        }

        protected void NotifyAllPropertiesChanged()
        {
            NotifyPropertyChanged(null);
        }

        protected bool SetProperty<T>(
            ref T backingStore, 
            T value,
            [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            NotifyPropertyChanged(propertyName);

            return true;
        }

        protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnCultureChanged(CultureInfo culture)
        {
        }

        private void CultureChanged(object sender, CultureChangeEventArgs args)
        {
            OnCultureChanged(args.NewCulture);
        }
    }
}