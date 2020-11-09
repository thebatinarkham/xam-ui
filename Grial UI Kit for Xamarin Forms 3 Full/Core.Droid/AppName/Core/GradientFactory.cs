using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using System;
using Xamarin.Forms.Platform.Android;

namespace AppName.Core
{
    internal static class GradientFactory
    {
        private class Radius
        {
            private readonly RadialGradient _radial;

            private readonly Context _context;

            public Radius(RadialGradient radial, Context context)
            {
                _radial = radial;
                _context = context;
            }

            public float GetX(int width)
            {
                double? num = _radial.RadiusX ?? _radial.Radius;
                if (_radial.RadiusType == RadiusType.Absolute)
                {
                    if (num.HasValue)
                    {
                        return _context.ToPixels(Math.Abs(num.Value));
                    }
                    return width / 2;
                }
                return (float)Math.Abs(num ?? 0.5) * (float)width;
            }

            public float GetY(int height)
            {
                double? num = _radial.RadiusY ?? _radial.Radius;
                if (_radial.RadiusType == RadiusType.Absolute)
                {
                    if (num.HasValue)
                    {
                        return _context.ToPixels(Math.Abs(num.Value));
                    }
                    return height / 2;
                }
                return (float)Math.Abs(num ?? 0.5) * (float)height;
            }
        }

        private class RadialGradientShaderFactory : ShapeDrawable.ShaderFactory
        {
            private readonly int[] _colors;

            private readonly float[] _positions;

            private readonly float _centerX;

            private readonly float _centerY;

            private readonly Radius _radius;

            private readonly double _fixedHeight;

            public RadialGradientShaderFactory(double centerX, double centerY, Radius radius, int[] colors, float[] positions, double fixedHeight)
            {
                _colors = colors;
                _positions = positions;
                _centerX = (float)centerX;
                _centerY = (float)centerY;
                _radius = radius;
                _fixedHeight = fixedHeight;
            }

            public override Shader Resize(int width, int height)
            {
                if (_fixedHeight > 0.0)
                {
                    height = (int)_fixedHeight;
                }
                float x = _radius.GetX(width);
                if (x <= 0f)
                {
                    return null;
                }
                float num = _centerX * (float)width;
                float num2 = _centerY * (float)height;
                Android.Graphics.RadialGradient radialGradient = new Android.Graphics.RadialGradient(num, num2, x, _colors, _positions, Shader.TileMode.Clamp);
                Matrix matrix = new Matrix();
                float y = _radius.GetY(height);
                matrix.PostScale(1f, y / x, num, num2);
                radialGradient.SetLocalMatrix(matrix);
                return radialGradient;
            }
        }

        private class LinearGradientShaderFactory : ShapeDrawable.ShaderFactory
        {
            private readonly int[] _colors;

            private readonly float[] _positions;

            private readonly float _angle;

            private readonly double _fixedHeight;

            public LinearGradientShaderFactory(double angle, int[] colors, float[] positions, double fixedHeight)
            {
                _colors = colors;
                _positions = positions;
                _angle = (float)angle;
                _fixedHeight = fixedHeight;
            }

            public override Shader Resize(int width, int height)
            {
                if (_fixedHeight > 0.0)
                {
                    height = (int)_fixedHeight;
                }
                Android.Graphics.LinearGradient linearGradient = new Android.Graphics.LinearGradient(0f, height, 0f, 0f, _colors, _positions, Shader.TileMode.Clamp);
                Matrix matrix = new Matrix();
                matrix.PostRotate(_angle, width / 2, height / 2);
                if (width != height && height > 0)
                {
                    matrix.PostScale((float)width / (float)height, 1f, width / 2, height / 2);
                }
                linearGradient.SetLocalMatrix(matrix);
                return linearGradient;
            }
        }

        public static PaintDrawable GetDrawable(Gradient gradient, Context context, double fixedHeight = -1.0)
        {
            ShapeDrawable.ShaderFactory shaderFactory = BuildShaderFactory(gradient, context, fixedHeight);
            if (shaderFactory == null)
            {
                return null;
            }
            PaintDrawable paintDrawable = new PaintDrawable();
            paintDrawable.SetShaderFactory(shaderFactory);
            paintDrawable.Shape = new RectShape();
            return paintDrawable;
        }

        private static ShapeDrawable.ShaderFactory BuildShaderFactory(Gradient gradient, Context context, double fixedHeight)
        {
            int[] newColors = new int[gradient.Colors.Count];
            float[] newPositions = new float[gradient.Colors.Count];
            float num = -1f;
            for (int i = 0; i < gradient.Colors.Count; i++)
            {
                GradientColor gradientColor = gradient.Colors[i];
                newColors[i] = gradientColor.FinalColor.ToAndroid();
                if (newPositions != null)
                {
                    double? position = gradientColor.Position;
                    if (!position.HasValue || (double)num > position || position < 0.0 || position > 1.0)
                    {
                        newPositions = null;
                    }
                    else
                    {
                        num = (newPositions[i] = (float)position.Value);
                    }
                }
            }
            fixedHeight = ((fixedHeight > 0.0) ? context.ToPixels(fixedHeight) : (-1f));
            LinearGradient linearGradient = gradient as LinearGradient;
            if (linearGradient != null)
            {
                double num2 = linearGradient.Angle % 360.0;
                if (num2 < 0.0)
                {
                    num2 += 360.0;
                }
                EnsureAtLeast2Colors(newColors, newPositions, out newColors, out newPositions);
                return new LinearGradientShaderFactory(num2, newColors, newPositions, fixedHeight);
            }
            RadialGradient radialGradient = gradient as RadialGradient;
            if (radialGradient != null)
            {
                EnsureAtLeast2Colors(newColors, newPositions, out newColors, out newPositions);
                if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop && newPositions != null && newPositions[newPositions.Length - 1] != 1f)
                {
                    float[] array = newPositions;
                    newPositions = new float[array.Length + 1];
                    Array.Copy(array, newPositions, array.Length);
                    newPositions[array.Length] = 1f;
                    int[] array2 = newColors;
                    newColors = new int[array2.Length + 1];
                    Array.Copy(array2, newColors, array2.Length);
                    newColors[array2.Length] = newColors[array2.Length - 1];
                }
                return new RadialGradientShaderFactory(radialGradient.CenterX, radialGradient.CenterY, new Radius(radialGradient, context), newColors, newPositions, fixedHeight);
            }
            return null;
        }

        private static void EnsureAtLeast2Colors(int[] colors, float[] positions, out int[] newColors, out float[] newPositions)
        {
            newColors = colors;
            newPositions = positions;
            if (colors.Length <= 1)
            {
                positions = null;
                newColors = new int[2];
                if (colors.Length == 1)
                {
                    newColors[0] = colors[0];
                    newColors[1] = colors[0];
                }
                else
                {
                    newColors[0] = Xamarin.Forms.Color.Transparent.ToAndroid();
                    newColors[1] = Xamarin.Forms.Color.Transparent.ToAndroid();
                }
            }
        }
    }
}
