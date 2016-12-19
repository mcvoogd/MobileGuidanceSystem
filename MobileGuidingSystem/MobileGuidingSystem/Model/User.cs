using System;
using Windows.Devices.Geolocation;

namespace MobileGuidingSystem.Model
{
    public class User
    {
        public Geopoint location { get; set; }
        public string Language;
        private Geolocator geolocator;

        public User()
        {
            geolocator = new Geolocator();
           // GetCurrentPosition();
        }

        public async void GetCurrentPosition()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    var p = geolocator.GetGeopositionAsync();
                    location = p.GetResults().Coordinate.Point;
            break;

                case GeolocationAccessStatus.Denied:
            break;

                case GeolocationAccessStatus.Unspecified:
            break;
            }
        }


        }
    }
