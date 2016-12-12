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
            mapcontrol.Center = user.location;
        }

        public override void NextPage(object user)
        {
            
        }
    }
}