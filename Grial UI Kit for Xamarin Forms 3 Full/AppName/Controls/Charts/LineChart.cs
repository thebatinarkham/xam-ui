using System.Collections;
using Xamarin.Forms;

namespace AppName
{
    public class LineChart : ChartViewAdapter<Microcharts.LineChart>
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                "ItemsSource",
                typeof(IEnumerable),
                typeof(LineChart),
                default(IEnumerable),
                propertyChanged: ItemSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        //LineMode
        public static readonly BindableProperty LineModeProperty =
            BindableProperty.Create(
                "LineMode",
                typeof(Microcharts.LineMode),
                typeof(LineChart),
                Microcharts.LineMode.Spline,
                propertyChanged: LineModeChanged);

        public Microcharts.LineMode LineMode
        {
            get { return (Microcharts.LineMode)GetValue(LineModeProperty); }
            set { SetValue(LineModeProperty, value); }
        }

        protected override Microcharts.LineChart CreateChart()
        {
            return new Microcharts.LineChart();
        }

        protected override void SetSpecificProperties()
        {
            var chart = GetChart(true);

            chart.Entries = ConvertAll(ItemsSource);
            chart.LineMode = LineMode;
        }

        private static void ItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (LineChart)bindable;
            var chart = adapter.GetChart();

            if (chart != null)
            {
                chart.Entries = adapter.ConvertAll((IEnumerable)newValue);
            }
        }

        private static void LineModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (LineChart)bindable;
            var chart = adapter.GetChart();

            if (chart != null)
            {
                chart.LineMode = adapter.LineMode;
            }
        }
    }
}
