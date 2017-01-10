using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Windows.Globalization;
#pragma warning disable 649
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;

namespace MobileGuidingSystem.Model.Data
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Sight
    {
        public string title;
        public string description;
        public string description_NL;
        public string description_EN;
        public string[] images;

        public string address;
        public double longitude;
        public double latitude;

        private List<RandomAccessStreamReference> _randomAccessStreamReferences;
        private List<string> _fullImagePaths;

        public string Name => title;

        public string Description
        {
            get
            {
                var locale = ApplicationLanguages.PrimaryLanguageOverride;

                return locale == "en-US"
                           ? (description_EN ?? "No translation available")
                           : (description_NL ?? description);
            }
        }

        public List<string> ImagePaths => images == null || images.Length == 0 ? new List<string> { "NoImage.png" } : new List<string>(images);

        public Geopoint Position => new Geopoint(new BasicGeoposition() { Latitude = latitude, Longitude = longitude });
        public string Address => address;

        public bool viewed = false;

        public List<RandomAccessStreamReference> ImageStreamReferences
        {
            get
            {
                if (_randomAccessStreamReferences != null) return _randomAccessStreamReferences;
                List<RandomAccessStreamReference> l = ImagePaths.Select(imagePath => RandomAccessStreamReference.CreateFromUri(Utils.MakeUri("Pictures/" + imagePath))).ToList();
                _randomAccessStreamReferences = l;
                return _randomAccessStreamReferences;
            }
        }

        public RandomAccessStreamReference Icon => ImageStreamReferences[0];

        public List<string> FullImagePaths
        {
            get
            {
                if (_fullImagePaths == null)
                {
                    var tmpList = new List<string>();
                    ImagePaths.ForEach(i => tmpList.Add($"ms-appx:///Assets/Pictures/{i}"));
                    _fullImagePaths = tmpList;
                }
                return _fullImagePaths;
            }
        }
    }
}