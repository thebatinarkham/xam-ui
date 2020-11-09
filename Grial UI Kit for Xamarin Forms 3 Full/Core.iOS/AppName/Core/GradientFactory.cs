using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms.Platform.iOS;

namespace AppName.Core
{
    internal static class GradientFactory
    {
        public static CAGradientLayer GetLayer(Gradient gradient)
        {
            CAGradientLayer cAGradientLayer = new CAGradientLayer();
            UpdateLayer(gradient, cAGradientLayer);
            return cAGradientLayer;
        }

        public static void UpdateLayer(Gradient gradient, CAGradientLayer gradientLayer)
        {
            Collection<GradientColor> collection = gradient.Colors;
            bool flag = false;
            if (collection.Count == 1)
            {
                collection = new Collection<GradientColor> {
                gradient.Colors [0],
                gradient.Colors [0]
            };
                flag = true;
            }
            CGColor[] array = new CGColor[collection.Count];
            NSNumber[] array2 = new NSNumber[collection.Count];
            double num = -1.0;
            for (int i = 0; i < collection.Count; i++)
            {
                GradientColor gradientColor = collection[i];
                array[i] = gradientColor.FinalColor.ToCGColor();
                if (array2 != null)
                {
                    double? position = gradientColor.Position;
                    if (flag || !position.HasValue || num > position || position < 0.0 || position > 1.0)
                    {
                        array2 = null;
                        continue;
                    }
                    num = position.Value;
                    array2[i] = new NSNumber(num);
                }
            }
            gradientLayer.Colors = array;
            gradientLayer.Locations = array2;
            LinearGradient linearGradient = gradient as LinearGradient;
            if (linearGradient != null)
            {
                gradientLayer.LayerType = CAGradientLayerType.Axial;
                double num2 = linearGradient.Angle % 360.0;
                if (num2 < 0.0)
                {
                    num2 += 360.0;
                }
                double num4;
                double num3;
                double num5;
                double num6;
                if (num2 == 0.0)
                {
                    num4 = (num3 = 0.0);
                    num5 = 1.0;
                    num6 = 0.0;
                }
                else if (num2 == 180.0)
                {
                    num4 = (num3 = 0.0);
                    num5 = 0.0;
                    num6 = 1.0;
                }
                else if (num2 == 90.0)
                {
                    num5 = (num6 = 0.0);
                    num4 = 0.0;
                    num3 = 1.0;
                }
                else if (num2 == 270.0)
                {
                    num5 = (num6 = 0.0);
                    num4 = 1.0;
                    num3 = 0.0;
                }
                else
                {
                    double a = (90.0 - num2) / 180.0 * Math.PI;
                    double slope = Math.Tan(a);
                    double b = 0.5 - 0.5 * slope;
                    Func<double, double> func = (double x) => slope * x + b;
                    Func<double, double> func2 = (double y) => (y - b) / slope;
                    if (func(0.0) > 1.0)
                    {
                        num4 = func2(1.0);
                        num3 = func2(0.0);
                        num5 = 0.0;
                        num6 = 1.0;
                    }
                    else if (func(0.0) < 0.0)
                    {
                        num4 = func2(0.0);
                        num3 = func2(1.0);
                        num5 = 1.0;
                        num6 = 0.0;
                    }
                    else
                    {
                        num4 = 0.0;
                        num3 = 1.0;
                        num5 = func(1.0);
                        num6 = func(0.0);
                    }
                    if (num2 > 180.0)
                    {
                        double num7 = num3;
                        num3 = num4;
                        num4 = num7;
                        double num8 = num6;
                        num6 = num5;
                        num5 = num8;
                    }
                }
                gradientLayer.StartPoint = new CGPoint(num4, num5);
                gradientLayer.EndPoint = new CGPoint(num3, num6);
            }
            else
            {
                RadialGradient radialGradient = gradient as RadialGradient;
                if (radialGradient != null)
                {
                    gradientLayer.LayerType = CAGradientLayerType.Radial;
                    gradientLayer.StartPoint = new CGPoint(radialGradient.CenterX, radialGradient.CenterY);
                    UpdateRadialRadius(gradient, gradientLayer);
                }
            }
        }

        public static void UpdateRadialRadius(Gradient gradient, CAGradientLayer gradientLayer)
        {
            RadialGradient radialGradient = gradient as RadialGradient;
            if (radialGradient != null)
            {
                double value;
                double value2;
                if (radialGradient.RadiusType == RadiusType.Absolute)
                {
                    value = (radialGradient.RadiusX ?? radialGradient.Radius ?? ((double)(gradientLayer.Frame.Width / 2))) / (double)gradientLayer.Frame.Width;
                    value2 = (radialGradient.RadiusY ?? radialGradient.Radius ?? ((double)(gradientLayer.Frame.Height / 2))) / (double)gradientLayer.Frame.Height;
                }
                else
                {
                    value = (radialGradient.RadiusX ?? radialGradient.Radius ?? 0.5);
                    value2 = (radialGradient.RadiusY ?? radialGradient.Radius ?? 0.5);
                }
                gradientLayer.EndPoint = new CGPoint((double)gradientLayer.StartPoint.X + Math.Abs(value), (double)gradientLayer.StartPoint.Y + Math.Abs(value2));
            }
        }
    }
}
