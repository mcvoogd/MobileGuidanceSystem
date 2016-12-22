using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileGuidingSystem.Model.Data
{
    public class Route
    {
        public string Name;

        public List<Sight> Sights => _sights ?? (_sights = LoadRoute());

        private List<Sight> LoadRoute() => JsonConvert.DeserializeObject<List<Sight>>(Utils.ReadJsonFile(Filename));

        public string Filename;
        private List<Sight> _sights;

        //Route Name : File Name
        public static List<Route> Routes = new List<Route>();

        static Route()
        {
            Routes.Add(new Route { Name = "Blindwalls", Filename = "blindwalls.json"});
            //TODO: Add historische kilometer filename
            Routes.Add(new Route {Name= "Historische Kilometer", Filename = "HistorischeKM.json"});
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

