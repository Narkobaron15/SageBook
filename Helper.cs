using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ADO.NET_Homework_3
{
    public static class Helper
    {
        public static BitmapImage? TryGetImageFromByteArray(this byte[] blob)
        {
            if (blob is null || blob.Length == 0) return null;

            using MemoryStream stream = new(blob);
            BitmapImage image = new();

            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();

            return image;
        }
    }
}
