using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AppName.Core;

namespace AppName
{
    public partial class Rating : ContentView
    {
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(
                nameof(Value),
                typeof(double),
                typeof(Rating),
                0.0,
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (Rating)bindable;
                    ctrl.RatingLabel.Text = ctrl.Update();
                });

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly BindableProperty MaxProperty =
            BindableProperty.Create(
                nameof(Max),
                typeof(double),
                typeof(Rating),
                5.0,
                defaultBindingMode: BindingMode.OneWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var ctrl = (Rating)bindable;
                    ctrl.RatingLabel.Text = ctrl.Update();
                });

        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        private const string EmptyIconChar = GrialIconsFont.Star;
        private const string PartialIconChar = GrialIconsFont.StarHalf;
        private const string FullIconChar = GrialIconsFont.StarFill;

        public Rating()
        {
            InitializeComponent();
        }

        public string Update()
        {
            var str = string.Empty;
            var value = Value;

            if (Value > Max)
            {
                value = Max;
            }
            else if (Value < 0)
            {
                value = 0;
            }

            for (var i = 1; i <= Max; i++)
            {
                // i <= value
                if (i < value || Math.Abs((double)i - value) < 0.01)
                {
                    str += FullIconChar;
                }
                else
                {
                    if (i - value > 1.0)
                    {
                        str += EmptyIconChar;
                    }
                    else
                    {
                        var decimals = value - Math.Floor(value);

                        if (decimals < 0.2)
                        {
                            str += EmptyIconChar;
                        }
                        else if (decimals > 0.8)
                        {
                            str += FullIconChar;
                        }
                        else
                        {
                            str += PartialIconChar;
                        }
                    }
                }
            }

            return str;
        }
    }
}