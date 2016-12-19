using System;
using Windows.Devices.Geolocation;

namespace MobileGuidingSystem.Model
{
    public class User
    {
        public Geopoint location { get; set; }
        public string Language;
        private Geolocator geolocator;
        public delegate void positionChangedDelegate(object sender, PositionChangedEventArgs e);
        public positionChangedDelegate positionChangedNotifier;

        public User()
        {
            // GetCurrentPosition();
        }


        }
    }
