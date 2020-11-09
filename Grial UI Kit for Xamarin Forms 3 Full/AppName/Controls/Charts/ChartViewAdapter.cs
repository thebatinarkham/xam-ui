using System;
using System.Collections;
using System.Collections.Generic;
using Microcharts;
using Microcharts.Forms;
using SkiaSharp;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppName
{
    public abstract class ChartViewAdapter : ChartView
    {
        public static readonly BindableProperty ItemsAdapterProperty =
            BindableProperty.Create(
                nameof(ItemsAdapter),
                typeof(Func<object, Microcharts.ChartEntry>),
                typeof(ChartViewAdapter),
                null);

        public Func<object, Microcharts.ChartEntry> ItemsAdapter
        {
            get { return (Func<object, Microcharts.ChartEntry>)GetValue(ItemsAdapterProperty); }
            set { SetValue(ItemsAdapterProperty, value); }
        }

        // ShowValueLabel
        public static readonly BindableProperty ShowValueLabelProperty =
            BindableProperty.Create(
                nameof(ShowValueLabel),
                typeof(bool),
                typeof(ChartViewAdapter),
                true);

        public bool ShowValueLabel
        {
            get { return (bool)GetValue(ShowValueLabelProperty); }
            set { SetValue(ShowValueLabelProperty, value); }
        }

        // IsAnimated
        public static readonly BindableProperty IsAnimatedProperty =
            BindableProperty.Create(
                nameof(IsAnimated),
                typeof(bool),
                typeof(ChartViewAdapter),
                false,
                propertyChanged: IsAnimatedChanged);

        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        // AnimationDuration
        public static readonly BindableProperty AnimationDurationProperty =
            BindableProperty.Create(
                nameof(AnimationDuration),
                typeof(TimeSpan),
                typeof(ChartViewAdapter),
                TimeSpan.FromSeconds(0.2));

        public TimeSpan AnimationDuration
        {
            get { return (TimeSpan)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        // LabelColor
        public static readonly BindableProperty LabelColorProperty =
            BindableProperty.Create(
                nameof(LabelColor),
                typeof(Color),
                typeof(ChartViewAdapter),
                Color.Gray,
                propertyChanged: LabelColorChanged);

        public Color LabelColor
        {
            get { return (Color)GetValue(LabelColorProperty); }
            set { SetValue(LabelColorProperty, value); }
        }

        // LabelTextSize
        public static readonly BindableProperty LabelTextSizeProperty =
            BindableProperty.Create(
                nameof(LabelTextSize),
                typeof(double),
                typeof(ChartViewAdapter),
                12.0,
                propertyChanged: LabelTextSizeChanged);

        public double LabelTextSize
        {
            get { return (double)GetValue(LabelTextSizeProperty) * DevicePixelsDensity; }
            set { SetValue(LabelTextSizeProperty, value); }
        }

        // ValueLabelOrientation
        public static readonly BindableProperty ValueLabelOrientationProperty =
            BindableProperty.Create(
                nameof(LabelOrientation),
                typeof(Microcharts.Orientation),
                typeof(ChartViewAdapter),
                Microcharts.Orientation.Horizontal,
                propertyChanged: ValueLabelOrientationChanged);

        public Microcharts.Orientation ValueLabelOrientation
        {
            get { return (Microcharts.Orientation)GetValue(ValueLabelOrientationProperty); }
            set { SetValue(LabelTextSizeProperty, value); }
        }

        // LabelOrientation
        public static readonly BindableProperty LabelOrientationProperty =
            BindableProperty.Create(
                nameof(LabelOrientation),
                typeof(Microcharts.Orientation),
                typeof(ChartViewAdapter),
                Microcharts.Orientation.Horizontal,
                propertyChanged: LabelOrientationChanged);

        public Microcharts.Orientation LabelOrientation
        {
            get { return (Microcharts.Orientation)GetValue(LabelOrientationProperty); }
            set { SetValue(LabelTextSizeProperty, value); }
        }

        // BackgroundColor
        public static readonly new BindableProperty BackgroundColorProperty =
            BindableProperty.Create(
                nameof(BackgroundColor),
                typeof(Color),
                typeof(ChartViewAdapter),
                Color.Transparent,
                propertyChanged: BackgroundColorChanged);

        public new Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        // BarAreaAlpha
        public static readonly BindableProperty AreaAlphaProperty =
            BindableProperty.Create(
                nameof(AreaAlpha),
                typeof(double),
                typeof(ChartViewAdapter),
                1d,
                propertyChanged: AreaAlphaChanged);

        public double AreaAlpha
        {
            get { return (double)GetValue(AreaAlphaProperty); }
            set { SetValue(AreaAlphaProperty, value); }
        }

        // PointSize
        public static readonly BindableProperty PointSizeProperty =
            BindableProperty.Create(
                nameof(PointSize),
                typeof(double),
                typeof(ChartViewAdapter),
                1.0,
                propertyChanged: PointSizeChanged);

        public double PointSize
        {
            get { return (double)GetValue(PointSizeProperty) * DevicePixelsDensity; }
            set { SetValue(PointSizeProperty, value); }
        }

        // LineSize
        public static readonly BindableProperty LineSizeProperty =
            BindableProperty.Create(
                nameof(LineSize),
                typeof(double),
                typeof(ChartViewAdapter),
                1.0,
                propertyChanged: LineSizeChanged);

        public double LineSize
        {
            get { return (double)GetValue(LineSizeProperty) * DevicePixelsDensity; }
            set { SetValue(LineSizeProperty, value); }
        }

        // PointMode
        public static readonly BindableProperty PointModeProperty =
            BindableProperty.Create(
                nameof(PointMode),
                typeof(Microcharts.PointMode),
                typeof(ChartViewAdapter),
                Microcharts.PointMode.Circle,
                propertyChanged: PointModeChanged);

        public Microcharts.PointMode PointMode
        {
            get { return (Microcharts.PointMode)GetValue(PointModeProperty); }
            set { SetValue(PointModeProperty, value); }
        }

        // MaxValue
        public static readonly BindableProperty MaxValueProperty =
            BindableProperty.Create(
                nameof(MaxValue),
                typeof(float),
                typeof(ChartViewAdapter),
                100f,
                propertyChanged: MaxValueChanged);

        public float MaxValue
        {
            get { return (float)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // MinimumValue
        public static readonly BindableProperty MinValueProperty =
            BindableProperty.Create(
                nameof(MinValue),
                typeof(float),
                typeof(RadialGaugeChart),
                0f,
                propertyChanged: MinValueChanged);

        public float MinValue
        {
            get { return (float)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        // UseValuesAsLabels
        public static readonly BindableProperty UseEntryValuesAsLabelsProperty =
            BindableProperty.Create(
                nameof(UseEntryValuesAsLabels),
                typeof(bool),
                typeof(ChartViewAdapter),
                true,
                propertyChanged: DefaultEntryColorChanged);

        public bool UseEntryValuesAsLabels
        {
            get { return (bool)GetValue(UseEntryValuesAsLabelsProperty); }
            set { SetValue(UseEntryValuesAsLabelsProperty, value); }
        }

        // DefaultEntryColor
        public static readonly BindableProperty DefaultEntryColorProperty =
            BindableProperty.Create(
                nameof(DefaultEntryColor),
                typeof(Color),
                typeof(ChartViewAdapter),
                Color.Default,
                propertyChanged: DefaultEntryColorChanged);

        public Color DefaultEntryColor
        {
            get { return (Color)GetValue(DefaultEntryColorProperty); }
            set { SetValue(DefaultEntryColorProperty, value); }
        }

        // DefaultEntryTextColor
        public static readonly BindableProperty DefaultEntryTextColorProperty =
            BindableProperty.Create(
                nameof(DefaultEntryColor),
                typeof(Color),
                typeof(ChartViewAdapter),
                Color.Default,
                propertyChanged: DefaultEntryTextColorChanged);

        public Color DefaultEntryTextColor
        {
            get { return (Color)GetValue(DefaultEntryTextColorProperty); }
            set { SetValue(DefaultEntryTextColorProperty, value); }
        }

        private static readonly ColorTypeConverter FormsConverter = new ColorTypeConverter();

        private bool _parentSet;
        private SKColor? _defaultEntrySKColor, _defaultEntryTextSKColor;

        protected ChartViewAdapter()
        {
            ItemsAdapter = ConvertToEntry;
        }

        protected double DevicePixelsDensity => DeviceDisplay.MainDisplayInfo.Density;

        protected override void OnParentSet()
        {
            base.OnParentSet();

            var oldParentSet = _parentSet;

            _parentSet = Parent != null;

            if (_parentSet && !oldParentSet)
            {
                Create();
            }
        }

        protected void Create()
        {
            if (_parentSet)
            {
                CreateInternal();
            }
        }

        protected abstract void CreateInternal();

        protected IEnumerable<Microcharts.ChartEntry> ConvertAll(IEnumerable items)
        {
            if (items != null)
            {
                if (ItemsAdapter == null)
                {
                    foreach (var item in items)
                    {
                        yield return (Microcharts.ChartEntry)item;
                    }
                }
                else
                {
                    foreach (var item in items)
                    {
                        yield return ItemsAdapter(item);
                    }
                }
            }
        }

        protected void SetCommonChartProperties(Chart chart)
        {
            chart.LabelColor = SKColor.Parse(LabelColor.ToHexString());
            chart.LabelTextSize = (float)LabelTextSize;
            chart.BackgroundColor = SKColor.Parse(BackgroundColor.ToHexString());
            chart.MaxValue = MaxValue;
            chart.MinValue = MinValue;

            chart.IsAnimated = IsAnimated;
            chart.AnimationDuration = AnimationDuration;

            if (chart is PointChart point)
            {
                point.LabelOrientation = LabelOrientation;
                point.ValueLabelOrientation = ValueLabelOrientation;
            }

            //BarChart
            if (chart is Microcharts.BarChart barChart)
            {
                barChart.BarAreaAlpha = (byte)(Math.Min(AreaAlpha, 1d) * 255);
                barChart.PointMode = PointMode;
                barChart.PointSize = (float)PointSize;
                barChart.LabelOrientation = LabelOrientation;
                barChart.ValueLabelOrientation = ValueLabelOrientation;
            }

            //LineChart
            if (chart is Microcharts.LineChart lineChart)
            {
                lineChart.PointSize = (float)PointSize;
                lineChart.LineSize = (float)LineSize;
                lineChart.LineAreaAlpha = (byte)(Math.Min(AreaAlpha, 1d) * 255);
                lineChart.PointMode = PointMode;
            }

            //RadialGaugeChart
            if (chart is Microcharts.RadialGaugeChart radialGaugeChart)
            {
                radialGaugeChart.LineSize = (float)LineSize;
                radialGaugeChart.LineAreaAlpha = (byte)(Math.Min(AreaAlpha, 1d) * 255);
            }

            //RadarChart
            if (chart is Microcharts.RadarChart radarChart)
            {
                radarChart.PointSize = (float)PointSize;
                radarChart.LineSize = (float)LineSize;
                radarChart.PointMode = PointMode;
            }
        }

        private static void DefaultEntryTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;

            adapter._defaultEntryTextSKColor = adapter.DefaultEntryTextColor != Color.Default ?
                SKColor.Parse(adapter.DefaultEntryTextColor.ToHexString()) :
                (SKColor?)null;
        }

        private static void DefaultEntryColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;

            adapter._defaultEntrySKColor = adapter.DefaultEntryColor != Color.Default ?
                SKColor.Parse(adapter.DefaultEntryColor.ToHexString()) :
                (SKColor?)null;
        }

        private static void IsAnimatedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null)
            {
                chart.IsAnimated = adapter.IsAnimated;
            }
        }

        private static void LabelColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null && adapter.LabelColor != Color.Default)
            {
                chart.LabelColor = SKColor.Parse(adapter.LabelColor.ToHexString());
            }
        }

        private static void LabelTextSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null)
            {
                chart.LabelTextSize = (float)adapter.LabelTextSize;
            }
        }

        private static void ValueLabelOrientationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null)
            {
                if (chart is PointChart point)
                {
                    point.ValueLabelOrientation = adapter.ValueLabelOrientation;
                }

                if (chart is Microcharts.BarChart barChart)
                {
                    barChart.ValueLabelOrientation = adapter.ValueLabelOrientation;
                }
            }
        }

        private static void LabelOrientationChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null)
            {
                if (chart is PointChart point)
                {
                    point.LabelOrientation = adapter.LabelOrientation;
                }

                if (chart is Microcharts.BarChart barChart)
                {
                    barChart.LabelOrientation = adapter.LabelOrientation;
                }
            }
        }

        private static void BackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null && adapter.BackgroundColor != Color.Default)
            {
                chart.BackgroundColor = SKColor.Parse(adapter.BackgroundColor.ToHexString());
            }
        }

        private static void AreaAlphaChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null)
            {
                if (chart is Microcharts.BarChart barChart)
                {
                    barChart.BarAreaAlpha = (byte)(Math.Min(adapter.AreaAlpha, 1d) * 255);
                }

                if (chart is Microcharts.LineChart lineChart)
                {
                    lineChart.LineAreaAlpha = (byte)(Math.Min(adapter.AreaAlpha, 1d) * 255);
                }

                if (chart is Microcharts.RadialGaugeChart radialGaugeChart)
                {
                    radialGaugeChart.LineAreaAlpha = (byte)(Math.Min(adapter.AreaAlpha, 1d) * 255);
                }
            }
        }

        private static void PointSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null)
            {
                if (chart is Microcharts.BarChart barChart)
                {
                    barChart.PointSize = (float)adapter.PointSize;
                }

                if (chart is Microcharts.LineChart lineChart)
                {
                    lineChart.PointSize = (float)adapter.PointSize;
                }

                if (chart is Microcharts.RadarChart radarChart)
                {
                    radarChart.PointSize = (float)adapter.PointSize;
                }
            }
        }

        private static void LineSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null)
            {
                if (chart is Microcharts.LineChart lineChart)
                {
                    lineChart.LineSize = (float)adapter.LineSize;
                }

                if (chart is Microcharts.RadialGaugeChart radialGaugeChart)
                {
                    radialGaugeChart.LineSize = (float)adapter.LineSize;
                }

                if (chart is Microcharts.RadarChart radarChart)
                {
                    radarChart.LineSize = (float)adapter.LineSize;
                }
            }
        }

        private static void PointModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null)
            {
                if (chart is Microcharts.BarChart barChart)
                {
                    barChart.PointMode = adapter.PointMode;
                }

                if (chart is Microcharts.LineChart lineChart)
                {
                    lineChart.PointMode = adapter.PointMode;
                }

                if (chart is Microcharts.RadarChart radarChart)
                {
                    radarChart.PointMode = adapter.PointMode;
                }
            }
        }

        private static void MaxValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null)
            {
                chart.MaxValue = adapter.MaxValue;
            }
        }

        private static void MinValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var adapter = (ChartViewAdapter)bindable;
            var chart = adapter.Chart;

            if (chart != null)
            {
                chart.MinValue = adapter.MinValue;
            }
        }

        private static bool ConvertColor(object color, out SKColor result)
        {
            result = default(SKColor);

            if (color != null)
            {
                if (color is SKColor)
                {
                    result = (SKColor)color;
                    return true;
                }

                var strColor = color as string;

                if (strColor != null)
                {
                    if (SKColor.TryParse(strColor, out result))
                    {
                        return true;
                    }

                    try
                    {
                        var xfColor = (Color)FormsConverter.ConvertFromInvariantString(strColor);

                        result = SKColor.Parse(xfColor.ToHexString());

                        return true;
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return false;
        }

        private Microcharts.ChartEntry ConvertToEntry(object elem)
        {
            Microcharts.ChartEntry result = null;

            if (elem != null)
            {
                result = elem as Microcharts.ChartEntry;

                if (result != null)
                {
                    if (!ShowValueLabel && result.ValueLabel != null)
                    {
                        result = new Microcharts.ChartEntry(result.Value)
                        {
                            Label = result.Label,
                            Color = result.Color,
                            TextColor = result.TextColor
                        };
                    }
                }
                else
                {
                    if (elem is Newtonsoft.Json.Linq.JObject jObject)
                    {
                        result = CreateFromJsonObject(jObject);
                    }
                    else
                    {
                        result = CreateFromType(elem);
                    }
                }
            }

            if (result != null && !ShowValueLabel)
            {
                result.ValueLabel = null;

                if (this is DonutChart || this is RadialGaugeChart)
                {
                    result.Label = null;
                }
            }

            return result;
        }

        private Microcharts.ChartEntry CreateFromJsonObject(Newtonsoft.Json.Linq.JObject obj)
        {
            float value = 0;
            string label = null;
            string valueLabel = null;
            bool colorSet = false;
            SKColor color = default(SKColor);
            bool textColorSet = false;
            SKColor textColor = default(SKColor);

            foreach (var prop in obj.Properties())
            {
                if (string.Equals(prop.Name, "Value", StringComparison.InvariantCultureIgnoreCase))
                {
                    value = (float)obj[prop.Name];
                }
                else if (string.Equals(prop.Name, "Label", StringComparison.InvariantCultureIgnoreCase))
                {
                    label = (string)obj[prop.Name];
                }
                else if (string.Equals(prop.Name, "ValueLabel", StringComparison.InvariantCultureIgnoreCase))
                {
                    valueLabel = (string)obj[prop.Name];
                }
                else if (string.Equals(prop.Name, "Color", StringComparison.InvariantCultureIgnoreCase))
                {
                    colorSet = ConvertColor((string)obj[prop.Name], out color);
                }
                else if (string.Equals(prop.Name, "TextColor", StringComparison.InvariantCultureIgnoreCase))
                {
                    textColorSet = ConvertColor((string)obj[prop.Name], out textColor);
                }
            }

            var result = new Microcharts.ChartEntry(value);

            if (label != null)
            {
                result.Label = label;
            }

            if (valueLabel != null)
            {
                result.ValueLabel = valueLabel;
            }
            else if (UseEntryValuesAsLabels)
            {
                result.ValueLabel = value.ToString();
            }

            if (colorSet)
            {
                result.Color = color;
            }
            else if (_defaultEntrySKColor != null)
            {
                result.Color = _defaultEntrySKColor.Value;
            }

            if (textColorSet)
            {
                result.TextColor = textColor;
            }
            else if (_defaultEntryTextSKColor != null)
            {
                result.TextColor = _defaultEntryTextSKColor.Value;
            }

            return result;
        }

        private Microcharts.ChartEntry CreateFromType(object elem)
        {
            var type = elem.GetType();

            float value = 0;
            bool valueSet = false;
            string label = null;
            string valueLabel = null;
            bool colorSet = false;
            SKColor color = default(SKColor);
            bool textColorSet = false;
            SKColor textColor = default(SKColor);

            foreach (var prop in type.GetProperties())
            {
                if (string.Equals(prop.Name, "Value", StringComparison.InvariantCultureIgnoreCase))
                {
                    var x = prop.GetValue(elem);
                    if (x is IConvertible)
                    {
                        value = (float)Convert.ToDouble(x);
                        valueSet = true;
                    }
                }
                else if (string.Equals(prop.Name, "Label", StringComparison.InvariantCultureIgnoreCase))
                {
                    label = (string)prop.GetValue(elem);
                }
                else if (string.Equals(prop.Name, "ValueLabel", StringComparison.InvariantCultureIgnoreCase))
                {
                    valueLabel = (string)prop.GetValue(elem);
                }
                else if (string.Equals(prop.Name, "Color", StringComparison.InvariantCultureIgnoreCase))
                {
                    colorSet = ConvertColor(prop.GetValue(elem), out color);
                }
                else if (string.Equals(prop.Name, "TextColor", StringComparison.InvariantCultureIgnoreCase))
                {
                    textColorSet = ConvertColor(prop.GetValue(elem), out textColor);
                }
            }

            if (!valueSet && elem is IConvertible)
            {
                value = (float)Convert.ToDouble(elem);
            }

            var result = new Microcharts.ChartEntry(value);

            if (label != null)
            {
                result.Label = label;
            }

            if (valueLabel != null)
            {
                result.ValueLabel = valueLabel;
            }
            else if (UseEntryValuesAsLabels)
            {
                result.ValueLabel = value.ToString();
            }

            if (colorSet)
            {
                result.Color = color;
            }
            else if (_defaultEntrySKColor != null)
            {
                result.Color = _defaultEntrySKColor.Value;
            }

            if (textColorSet)
            {
                result.TextColor = textColor;
            }
            else if (_defaultEntryTextSKColor != null)
            {
                result.TextColor = _defaultEntryTextSKColor.Value;
            }

            return result;
        }
    }

    public abstract class ChartViewAdapter<T> : ChartViewAdapter
        where T : Chart
    {
        private T _chart;

        protected override void CreateInternal()
        {
            var chart = GetChart(true);

            SetCommonChartProperties(chart);

            SetSpecificProperties();

            Chart = chart;
        }

        protected T GetChart(bool forceCreate = false)
        {
            if (_chart == null && forceCreate)
            {
                _chart = CreateChart();
            }

            return _chart;
        }

        protected abstract T CreateChart();

        protected abstract void SetSpecificProperties();
    }
}
