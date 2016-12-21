﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
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
            
            private List<RandomAccessStreamReference> _randomAccessStreamReferences;

            public string Name => title;
            public string Description => description;

            //TODO: Gooi ff naampie van t plaatie in da lege stringy
            public List<string> ImagePaths => images == null || images.Length == 0 ? new List<string> {""} : new List<string>(images);

            public Geopoint Position => new Geopoint(new BasicGeoposition() {Latitude = latitude, Longitude = longitude});
            public string Address => address;

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
        }
    }
}
