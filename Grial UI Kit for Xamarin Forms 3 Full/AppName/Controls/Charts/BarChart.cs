using System;
using System.Collections;
using Xamarin.Forms;

namespace AppName
{
    public class BarChart : ChartViewAdapter<Microcharts.BarChart>
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                "ItemsSource",
                typeof(IEnumerable),
                typeof(BarChart),
                default(IEnumerable),
                propertyChanged: ItemSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty PointAreaAlphaProperty =
            BindableProperty.Create(
                "PointAreaAlpha",
                typeof(double),
                typeof(BarChart),
                1d,
                propertyChanged: PointAreaAlphaChanged);

        public double PointAreaAlpha
        {
            get { return (double)GetValue(PointAreaAlphaProperty); }
            set { SetValue(PointAreaAlphaProperty, value); }
        }

        protected override Microcharts.BarChart CreateChart()
        {
            return new Microcharts.BarChart();
        }

        protected override void SetSpecificProperties()
        {
            var chart = GetChart(true);

            chart.Entries = ConvertAll(ItemsSource);
            chart.PointAreaAlpha = (byte)(Math.Min(PointAreaAlpha, 1d) * 255);
        }

        private static void ItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (BarChart)bindable;
            var chart = adapter.GetChart();

            if (chart != null)
            {
                chart.Entries = adapter.ConvertAll((IEnumerable)newValue);
            }
        }

        private static void PointAreaAlphaChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (BarChart)bindable;
            var chart = adapter.GetChart();

            if (chart != null)
            {
                chart.PointAreaAlpha = (byte)(Math.Min(adapter.PointAreaAlpha, 1d) * 255);
            }
        }
    }
}
