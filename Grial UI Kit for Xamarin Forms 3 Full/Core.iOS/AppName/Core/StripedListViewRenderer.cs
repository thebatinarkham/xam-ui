using System;
using AppName.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    internal class StripedListViewRenderer : ListViewRenderer
    {
        private DataGrid.StripedListView _listView;

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            Restore();
            base.OnElementChanged(e);
            Setup();
        }

        protected override void Dispose(bool disposing)
        {
            Restore();
            base.Dispose(disposing);
        }

        private void Setup()
        {
            DataGrid.StripedListView stripedListView = base.Element as DataGrid.StripedListView;
            if (stripedListView != null)
            {
                _listView = stripedListView;
                _listView.AutoSizedChanged -= OnAutoSizedChanged;
                _listView.AutoSizedChanged += OnAutoSizedChanged;
                Update();
            }
        }

        private void Restore()
        {
            if (_listView != null)
            {
                _listView.AutoSizedChanged -= OnAutoSizedChanged;
                _listView = null;
                if (base.Control != null)
                {
                    base.Control.ScrollEnabled = true;
                }
            }
        }

        private void OnAutoSizedChanged(object sender, EventArgs e)
        {
            Update();
        }

        private void Update()
        {
            if (base.Control != null)
            {
                base.Control.ScrollEnabled = !_listView.AutoSized;
            }
        }
    }
}
