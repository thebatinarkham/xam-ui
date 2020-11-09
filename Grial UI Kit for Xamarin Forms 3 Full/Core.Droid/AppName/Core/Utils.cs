using Android.Content.Res;
using Android.Graphics;

namespace AppName.Core
{
    public static class Utils
    {
        public static ColorStateList CreateColorStateList(Color normal, Color disabled, Color byDefault)
        {
            return new ColorStateList(new int[3][]
            {
                new int[1]
                {
                    16842912
                },
                new int[1]
                {
                    -16842910
                },
                new int[0]
            }, new int[3]
            {
                normal,
                disabled,
                byDefault
            });
        }

        public static ColorStateList CreateColorStateList(Color byDefault)
        {
            return new ColorStateList(new int[1][]
            {
                new int[0]
            }, new int[1]
            {
                byDefault
            });
        }
    }
}
