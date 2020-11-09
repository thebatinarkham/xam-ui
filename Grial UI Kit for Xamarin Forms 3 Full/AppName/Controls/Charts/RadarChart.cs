using System.Collections;
using SkiaSharp;
using Xamarin.Forms;

namespace AppName
{
    public class RadarChart : ChartViewAdapter<Microcharts.RadarChart>
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                "ItemsSource",
                typeof(IEnumerable),
                typeof(RadarChart),
                default(IEnumerable),
                propertyChanged: ItemSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        //BorderLineColor
        public static readonly BindableProperty BorderLineColorProperty =
            BindableProperty.Create(
                "BorderLineColor",
                typeof(Color),
                typeof(RadarChart),
                Color.Gray,
                propertyChanged: BorderLineColorChanged);

        public Color BorderLineColor
        {
            get { return (Color)GetValue(BorderLineColorProperty); }
            set { SetValue(BorderLineColorProperty, value); }
        }

        //BorderLineSize
        public static readonly BindableProperty BorderLineSizeProperty =
            BindableProperty.Create(
                "BorderLineSize",
                typeof(double),
                typeof(RadarChart),
                1.0,
                propertyChanged: BorderLineSizeChanged);

        public double BorderLineSize
        {
            get { return (double)GetValue(BorderLineSizeProperty) * DevicePixelsDensity; }
            set { SetValue(BorderLineSizeProperty, value); }
        }

        protected override Microcharts.RadarChart CreateChart()
        {
            return new Microcharts.RadarChart();
        }

        protected override void SetSpecificProperties()
        {
            var chart = GetChart(true);

            chart.Entries = ConvertAll(ItemsSource);
            chart.BorderLineColor = SKColor.Parse(BorderLineColor.ToHexString());
            chart.BorderLineSize = (float)BorderLineSize;
        }

        private static void ItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (RadarChart)bindable;
            var chart = adapter.GetChart();

            if (chart != null)
            {
                chart.Entries = adapter.ConvertAll((IEnumerable)newValue);
            }
        }

        private static void BorderLineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (RadarChart)bindable;
            var chart = adapter.GetChart();

            if (chart != null)
            {
                chart.BorderLineColor = SKColor.Parse(((Color)newValue).ToHexString());
            }
        }

        private static void BorderLineSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (RadarChart)bindable;
            var chart = adapter.GetChart();

            if (chart != null)
            {
                chart.BorderLineSize = (float)adapter.BorderLineSize;
            }
        }
    }
}
