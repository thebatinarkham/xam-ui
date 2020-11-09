using Android.Media;
using Android.Widget;

namespace AppName.Core
{
    internal class AndroidVideoView
    {
        public FormsVideoView VideoView
        {
            get;
            set;
        }

        public MediaController MediaController
        {
            get;
            set;
        }

        public MediaPlayer MediaPlayer
        {
            get;
            set;
        }
    }
}
