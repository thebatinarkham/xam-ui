using Xamarin.Forms;

namespace AppName
{
    public class RadialGaugeChart : ChartViewAdapter<Microcharts.RadialGaugeChart>
    {
        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(
                "Source",
                typeof(object),
                typeof(RadialGaugeChart),
                default(object),
                propertyChanged: SourceChanged);

        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly BindableProperty MaximumValueProperty =
            BindableProperty.Create(
                "MaximumValue",
                typeof(float),
                typeof(RadialGaugeChart),
                100f,
                propertyChanged: MaximumValueChanged);

        public float MaximumValue
        {
            get { return (float)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }

        protected override Microcharts.RadialGaugeChart CreateChart()
        {
            return new Microcharts.RadialGaugeChart();
        }

        protected override void SetSpecificProperties()
        {
            var chart = GetChart(true);

            chart.Entries = Source == null ? null : new Microcharts.ChartEntry[] { ItemsAdapter(Source) };
            chart.MaxValue = MaximumValue;
        }

        private static void SourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (RadialGaugeChart)bindable;
            var chart = adapter.GetChart();

            if (chart != null)
            {
                chart.Entries = adapter.Source == null ? null : new Microcharts.ChartEntry[] { adapter.ItemsAdapter(adapter.Source) };
            }
        }

        private static void MaximumValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (RadialGaugeChart)bindable;
            var chart = adapter.GetChart();

            if (chart != null)
            {
                chart.MaxValue = adapter.MaximumValue;
            }
        }
    }
}
