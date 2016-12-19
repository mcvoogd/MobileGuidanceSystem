using System;
using Windows.Storage;

namespace MobileGuidingSystem
{
    class Utils
    {
        public static StorageFile GetStorageFile(string fileName)
        {
            Uri uri = new Uri("ms-appx:///Assets/" + fileName);
            StorageFile anjFile = StorageFile.GetFileFromApplicationUriAsync(uri).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
            return anjFile;
        }

        public static string ReadJsonFile(string filename)
        {
            var file = GetStorageFile(filename);
            return FileIO.ReadTextAsync(file).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
