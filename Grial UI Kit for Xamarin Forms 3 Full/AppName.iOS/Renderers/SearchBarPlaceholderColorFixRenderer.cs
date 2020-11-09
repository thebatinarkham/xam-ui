using System;
using AppName.Core;
using Xamarin.Forms;

namespace AppName.iOS
{
	public class SearchBarPlaceholderColorFixRenderer : SearchBarRenderer
	{
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (Element != null)
            {
                var color = Element.PlaceholderColor;
                Element.PlaceholderColor = Color.Default;
                Element.PlaceholderColor = color;
            }
        }
    }
}
