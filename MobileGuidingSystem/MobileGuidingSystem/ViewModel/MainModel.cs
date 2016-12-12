using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using MobileGuidingSystem.Data;

namespace MobileGuidingSystem.ViewModel
{
    public class MainModel : Model
    {
        private MapControl _map; 

        public MainModel(MapControl mapcontrol)
        {
            _map = mapcontrol;
            User user = new User() {location = new Geopoint(new BasicGeoposition() {Latitude = 51.586267, Longitude = 4.780172 })};
            mapcontrol.Center = user.location;
        }

        public override void NextPage(object user)
        {
            
        }
    }
}