using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace MobileGuidingSystem
{
    class Utils
    {
        public static RandomAccessStreamReference GetFileStream(string fileName)
        {
            return RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/" + fileName));
        }
    }
}
