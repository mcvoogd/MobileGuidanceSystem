using Windows.Devices.Geolocation;

namespace MobileGuidingSystem.Model
{
    public class User
    {
        public delegate void PositionChangedDelegate(object sender, PositionChangedEventArgs e);

        private Geolocator _geolocator;
        public string Language;
        public PositionChangedDelegate PositionChangedNotifier;

        public Geopoint Location { get; set; }
    }
}