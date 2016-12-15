using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using MobileGuidingSystem.Data;

namespace MobileGuidingSystem.ViewModel
{
    public class MainModel : Model
    {
        private readonly MapControl _map;
        public Geopoint myLocation;
        private readonly bool onCollisionShow;
        public ObservableCollection<Sight> sights;

        public int zoomlevel { get { return 25; }}
        public int DesiredPitch { get { return 45;} }
        public MapStyle MapStyle = MapStyle.Road;

        private readonly Color RouteColor = Colors.Blue;
        private readonly Color OutlineColor = Colors.LightBlue;

        public MainModel(MapControl mapcontrol)
        {
            _map = mapcontrol;
            sights = new ObservableCollection<Sight>();
            GenerateSights();
            DrawRoutes(sights);
            myLocation = new Geopoint(new BasicGeoposition() {Latitude = 51.5860591, Longitude = 4.793500600000016 });
            user = new User();
            user.positionChangedNotifier += OnPositionChanged;
            //drawRoute(new Geopoint(new BasicGeoposition() { Latitude = 51.59000, Longitude = 4.781000 }), new Geopoint(new BasicGeoposition(){ Longitude = 4.780172, Latitude = 51.586267}) );

        }

        public async void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
           await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
             // _map.Center = e.Position.Coordinate.Point;
                myLocation = e.Position.Coordinate.Point;
            }
            );
        }

        public void GenerateSights()
        {
            Sight s1 = new Sight("Avans", "avans_logo.png", new Geopoint(new BasicGeoposition() {Latitude = 51.5860591, Longitude = 4.793500600000016 }));
            Sight s2 = new Sight("AMPHIA", "breda_logo.png", new Geopoint(new BasicGeoposition() {Latitude = 51.5819335, Longitude = 4.797045799999978 }));
            Sight s3 = new Sight("JOSHUA", "vvv_logo.png", new Geopoint(new BasicGeoposition() { Latitude = 51.5777335, Longitude = 4.789550899999995 }));

            sights.Add(s1);
            sights.Add(s2);
            sights.Add(s3);

        }

        public async void DrawRoute(Geopoint p1, Geopoint p2)
        {
            MapRouteFinderResult routeFinderResult = await MapRouteFinder.GetWalkingRouteAsync(p1, p2);

            if (routeFinderResult.Status == MapRouteFinderStatus.Success)
            {
                MapRouteView routeView = new MapRouteView(routeFinderResult.Route);
                routeView.RouteColor = RouteColor;
                routeView.OutlineColor = OutlineColor;
                _map.Routes.Add(routeView);
            }
        }

        public async void DrawRoutes(List<Geopoint> positions)
        {
            MapRouteFinderResult routeFinderResult = await MapRouteFinder.GetWalkingRouteFromWaypointsAsync(positions);

            if (routeFinderResult.Status == MapRouteFinderStatus.Success)
            {
                MapRouteView routeView = new MapRouteView(routeFinderResult.Route);
                routeView.RouteColor = RouteColor;
                routeView.OutlineColor = OutlineColor;
                _map.Routes.Add(routeView);
            }
        }

        public void DrawRoutes(ObservableCollection<Sight> sightlist)
        {
            List<Geopoint> positions = new List<Geopoint>();

            foreach (Sight s in sightlist)
            {
                positions.Add(s.Position);
            }

            DrawRoutes(positions);
        }

        public override void NextPage(object user)
        {
            throw new NotImplementedException();
        }

    }
}