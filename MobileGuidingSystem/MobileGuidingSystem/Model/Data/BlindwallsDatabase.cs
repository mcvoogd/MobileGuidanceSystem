using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Windows.Devices.Geolocation;
using Newtonsoft.Json;

namespace MobileGuidingSystem.Model.Data
{
    class BlindwallsDatabase
    {
        private static List<ISight> _sights;
        public static List<ISight> Sights
        {
            get
            {
                if (_sights == null || _sights.Count == 0)
                    LoadSights();
                return _sights;
            }
        }

        private static void LoadSights()
        {
            var bwSights = JsonConvert.DeserializeObject<List<BWSight>>(Utils.ReadJsonFile("JSON/blindwalls.json"));
            _sights = new List<ISight>(bwSights);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal class BWSight : ISight
        {
            public string title;
            public string description;
            public string[] images;

            public string address;
            public double longitude;
            public double latitude;
            public string Name => title;
            public string Description => description;
            public List<string> ImagePaths => new List<string>(images);
            public Geopoint Position => new Geopoint(new BasicGeoposition() {Latitude = latitude, Longitude = longitude});
            public string Address => address;
        }
    }
}
