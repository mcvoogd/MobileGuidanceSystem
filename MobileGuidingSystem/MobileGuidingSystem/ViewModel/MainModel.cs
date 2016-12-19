using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls.Maps;
using MobileGuidingSystem.Model;
using MobileGuidingSystem.Model.Data;

namespace MobileGuidingSystem.ViewModel
{
    public class MainModel : Model
    {
        private readonly MapControl _map;
        public Geopoint MyLocation;
        //private readonly bool _onCollisionShow;
        public ObservableCollection<ISight> Sights;

        public int Zoomlevel => 17;

        public int DesiredPitch => 45;

        public MapStyle MapStyle = MapStyle.Road;

        private readonly Color _routeColor = Colors.Blue;
        private readonly Color _outlineColor = Colors.LightBlue;
        public IRandomAccessStreamReference iconImage { get; set;}
        public Point Anchor { get; set; }
        

        public MainModel(MapControl mapcontrol)
        {
            _map = mapcontrol;
            Sights = new ObservableCollection<ISight>();
            LoadData();
            MyLocation = new Geopoint(new BasicGeoposition() { Latitude = 51.5860591, Longitude = 4.793500600000016 });
            User = new User();
            DrawRoutes(BlindwallsDatabase.Sights);
            iconImage = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/home-pin.png"));
            Anchor = new Point(0.5, 1);
            //drawRoute(new Geopoint(new BasicGeoposition() { Latitude = 51.59000, Longitude = 4.781000 }), new Geopoint(new BasicGeoposition(){ Longitude = 4.780172, Latitude = 51.586267}) );
            //  _map.ZoomLevelChanged += _map_ZoomLevelChanged;
        }

        private void LoadData()
        {
            //BlindwallsDatabase.Sights.ForEach(s=>Sights.Add(s));
            foreach (ISight sight in BlindwallsDatabase.Sights)
            {
                drawSight(sight.Position, sight.Name);
            }
        }

        //private void _map_ZoomLevelChanged(MapControl sender, object args)
        //{
        //    if (_map.ZoomLevel < 17)
        //    {
        //        _map.ZoomLevel = 17;
        //    }
        //}

        public async void DrawRoute(Geopoint p1, Geopoint p2)
        {
            MapRouteFinderResult routeFinderResult = await MapRouteFinder.GetWalkingRouteAsync(p1, p2);

            if (routeFinderResult.Status == MapRouteFinderStatus.Success)
            {
                MapRouteView routeView = new MapRouteView(routeFinderResult.Route);
                routeView.RouteColor = _routeColor;
                routeView.OutlineColor = _outlineColor;
                _map.Routes.Add(routeView);
            }
        }

        public async void DrawRoutes(List<Geopoint> positions)
        {
            MapRouteFinderResult routeFinderResult = await MapRouteFinder.GetWalkingRouteFromWaypointsAsync(positions);

            if (routeFinderResult.Status == MapRouteFinderStatus.Success)
            {
                MapRouteView routeView = new MapRouteView(routeFinderResult.Route);
                routeView.RouteColor = _routeColor;
                routeView.OutlineColor = _outlineColor;
                _map.Routes.Add(routeView);
            }
            else if (routeFinderResult.Status == MapRouteFinderStatus.NoRouteFoundWithGivenOptions)
            {
                MapRouteView routeView = new MapRouteView(routeFinderResult.Route);
                routeView.RouteColor = _routeColor;
                routeView.OutlineColor = _outlineColor;
                _map.Routes.Add(routeView);
            }
        }

        public void DrawRoutes(List<ISight> sightlist)
        {
            List<Geopoint> positions = new List<Geopoint>();


            foreach (ISight s in sightlist)
            {
                positions.Add(s.Position);
            }
            DrawRoutes(positions);
        }

        public override void NextPage(object user)
        {
            throw new NotImplementedException();
        }

        public void drawSight(Geopoint position, String title)
        {
            var ancherpoint = new Point(0.5,1);
            var image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/home-pin.png"));
            var Sight = new MapIcon
            {
                Title = title,
                Location = position,
                NormalizedAnchorPoint = ancherpoint,
                Image = image,
                ZIndex = 4,
                CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible
            
            };
            _map.MapElements.Add(Sight);
        }

        public void DrawPlayer(Geoposition position)
        {

            var ancherpoint = new Point(0.5, 0.5);
            var image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/PlayerPin.png"));
            var player = new MapIcon
            {
                Title = "",
                Location = position.Coordinate.Point,
                NormalizedAnchorPoint = ancherpoint,
                Image = image,
                ZIndex = 13
            };
            User.Location = player.Location;
            UpdatePos();
            _map.MapElements.Add(player);
        }

        public void UpdatePos()
        {
            if (_map.MapElements != null)
            {
                foreach (MapElement element in _map.MapElements)
                {
                    if (element is MapIcon)
                    {
                        MapIcon player = (MapIcon) element;
                        if (player.ZIndex == 13)
                        {
                            int index = _map.MapElements.IndexOf(player);
                            _map.MapElements.RemoveAt(index);
                            break;
                        }
                    }

                }
            }
        }



    }
           

    }