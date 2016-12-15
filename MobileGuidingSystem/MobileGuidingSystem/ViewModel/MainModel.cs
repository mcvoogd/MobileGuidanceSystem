using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;
using MobileGuidingSystem.Data;

namespace MobileGuidingSystem.ViewModel
{
    public class MainModel : Model
    {
        private MapControl _map;
        public string s1 = "HEEEEYYYOOOOO";
        private readonly bool onCollisionShow;

        public MainModel(MapControl mapcontrol)
        {
            _map = mapcontrol;
            InitializeMap();
            drawRoute(new Geopoint(new BasicGeoposition() { Latitude = 51.59000, Longitude = 4.781000 }), new Geopoint(new BasicGeoposition(){ Longitude = 4.780172, Latitude = 51.586267}) );
            drawWayPoint("home-pin.png", "HOME", new Geopoint(new BasicGeoposition() { Latitude = 51.59000, Longitude = 4.781000 }));
        }

        public override void NextPage(object user)
        {
            
        }

        public void InitializeMap()
        {
            user = new User();
            if (user.location == null)
            {
                user.location = new Geopoint(new BasicGeoposition() { Longitude = 4.780172,  Latitude = 51.586267});
            }

            _map.Center = user.location;
            _map.ZoomLevel = 25;
            _map.DesiredPitch = 45;
            _map.Style = MapStyle.Road;
            _map.LandmarksVisible = true;
        }

        public async void drawRoute(Geopoint p1, Geopoint p2)
        {
            MapRouteFinderResult routeResult = await MapRouteFinder.GetWalkingRouteAsync(p1, p2);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                MapRouteView viewOfMapRouteView = new MapRouteView(routeResult.Route);
                viewOfMapRouteView.RouteColor = Colors.Blue;
                viewOfMapRouteView.OutlineColor = Colors.LightBlue;
                _map.Routes.Add(viewOfMapRouteView);
            }
        }

        public void drawRoutes(List<Geopoint> positions)
        {
            Geopoint last = null;

            for(int i = 0; i < positions.Count; i++)
            {
                if (i == 0)
                {
                    last = positions[i];
                }
                else
                {
                    drawRoute(last, positions[i]);
                    last = positions[i];
                }
            }
        }

        public void drawWayPoint(string fileName, string title, Geopoint location)
        {
            var anchorPoint = new Point(0.5, 1);
            var image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/" + fileName));

            var shape = new MapIcon
            {
                Title = title,
                Location = location,
                NormalizedAnchorPoint = anchorPoint,
                Image = image,
                ZIndex = 20
            };


            shape.AddData(new Sight());

            _map.MapElements.Add(shape);
            _map.Center = location;
        }


        public void MyMap_OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            MapIcon myClickedIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            switch (myClickedIcon.CollisionBehaviorDesired)
            {
                    case MapElementCollisionBehavior.Hide:
                    myClickedIcon.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
                    break;

                    case MapElementCollisionBehavior.RemainVisible:
                    myClickedIcon.CollisionBehaviorDesired = MapElementCollisionBehavior.Hide;
                    break;
            }
        }
    }
}