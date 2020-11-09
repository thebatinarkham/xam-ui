using System.Collections;
using Xamarin.Forms;

namespace AppName
{
    public class DonutChart : ChartViewAdapter<Microcharts.DonutChart>
    {
        public static readonly BindableProperty HoleRadiusProperty =
            BindableProperty.Create(
                "HoleRadius",
                typeof(float),
                typeof(DonutChart),
                default(float),
                propertyChanged: HoleRadiusChanged);

        public float HoleRadius
        {
            get { return (float)GetValue(HoleRadiusProperty); }
            set { SetValue(HoleRadiusProperty, value); }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                "ItemsSource",
                typeof(IEnumerable),
                typeof(DonutChart),
                default(IEnumerable),
                propertyChanged: ItemSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        protected override Microcharts.DonutChart CreateChart()
        {
            return new Microcharts.DonutChart();
        }

        protected override void SetSpecificProperties()
        {
            var chart = GetChart(true);

            chart.HoleRadius = HoleRadius;
            chart.Entries = ConvertAll(ItemsSource);
        }

        private static void ItemSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (DonutChart)bindable;
            var chart = adapter.GetChart();

            if (chart != null)
            {
                chart.Entries = adapter.ConvertAll((IEnumerable)newValue);
            }
        }

        private static void HoleRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (DonutChart)bindable;
            var chart = adapter.GetChart();

            if (chart != null)
            {
                chart.HoleRadius = (float)newValue;
            }
        }
    }
}
