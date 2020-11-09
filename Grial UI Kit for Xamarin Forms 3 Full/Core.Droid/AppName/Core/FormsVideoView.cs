using Android.Content;
using Android.Media;
using Android.Net;
using Android.Widget;
using System.Collections.Generic;
using System.IO;

namespace AppName.Core
{
    internal class FormsVideoView : VideoView
    {
        private int videoHeight;

        private int videoWidth;

        public int VideoHeight => videoHeight;

        public int VideoWidth => videoWidth;

        public FormsVideoView(Context context)
            : base(context)
        {
        }

        public override void SetVideoPath(string path)
        {
            base.SetVideoPath(path);
            if (File.Exists(path))
            {
                MediaMetadataRetriever mediaMetadataRetriever = new MediaMetadataRetriever();
                try
                {
                    mediaMetadataRetriever.SetDataSource(path);
                    ExtractMetadata(mediaMetadataRetriever);
                }
                catch
                {
                }
            }
        }

        public override void SetVideoURI(Uri uri, IDictionary<string, string> headers)
        {
            GetMetaData(uri, headers);
            base.SetVideoURI(uri, headers);
        }

        public override void SetVideoURI(Uri uri)
        {
            GetMetaData(uri, new Dictionary<string, string>());
            base.SetVideoURI(uri);
        }

        private void GetMetaData(Uri uri, IDictionary<string, string> headers)
        {
            MediaMetadataRetriever mediaMetadataRetriever = new MediaMetadataRetriever();
            try
            {
                if (uri.Scheme != null && uri.Scheme.StartsWith("http") && headers != null)
                {
                    mediaMetadataRetriever.SetDataSource(uri.ToString(), headers);
                }
                else
                {
                    mediaMetadataRetriever.SetDataSource(base.Context, uri);
                }
                ExtractMetadata(mediaMetadataRetriever);
            }
            catch
            {
            }
        }

        private void ExtractMetadata(MediaMetadataRetriever retriever)
        {
            videoWidth = 0;
            int.TryParse(retriever.ExtractMetadata(MetadataKey.VideoWidth), out videoWidth);
            videoHeight = 0;
            int.TryParse(retriever.ExtractMetadata(MetadataKey.VideoHeight), out videoHeight);
        }
    }
}
