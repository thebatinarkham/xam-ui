using System.Collections;
using System.Linq;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public class ListViewLimitCountBehavior : Behavior<ListView>
    {
        public static readonly BindableProperty CountLimitProperty =
            BindableProperty.Create(
                nameof(CountLimit),
                typeof(int),
                typeof(Badge),
                defaultValue: -1,
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: OnCountLimitChanged);

        public int CountLimit
        {
            get { return (int)GetValue(CountLimitProperty); }
            set { SetValue(CountLimitProperty, value); }
        }

        private ListView _listView;
        private IEnumerable _originalSource;

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);

            _listView = bindable;

            if (_listView != null)
            {
                _listView.PropertyChanged += OnListViewPropertyChanged;
                _originalSource = _listView.ItemsSource;

                Update();
            }
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            if (_listView != null)
            {
                _listView.PropertyChanged -= OnListViewPropertyChanged;
                _listView = null;
            }

            base.OnDetachingFrom(bindable);
        }

        private static void OnCountLimitChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((ListViewLimitCountBehavior)bindable).Update();
        }

        private void OnListViewPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_originalSource == null && e.PropertyName == ListView.ItemsSourceProperty.PropertyName)
            {
                _originalSource = _listView.ItemsSource;
                Update();
            }
        }

        private void Update()
        {
            if (_listView != null && _originalSource != null)
            {
                int count;
                if (CountLimit < 0)
                {
                    _listView.ItemsSource = _originalSource;
                    count = _originalSource.Cast<object>().Count();
                }
                else
                {
                    var source = _originalSource.Cast<object>().Take(CountLimit).ToList();
                    count = source.Count;
                    _listView.ItemsSource = source;
                }

                UpdateHeight(count);
            }
        }

        private void UpdateHeight(int count)
        {
            const int ItemSeparation = 2;
            const int AbsolutePadding = 20;

            if (count == 0)
            {
                _listView.HeightRequest = -1;
            }
            else
            {
                _listView.HeightRequest = ((_listView.RowHeight + ItemSeparation) * count) + AbsolutePadding;
            }
        }
    }
}