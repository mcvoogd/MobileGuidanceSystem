using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using MobileGuidingSystem.Model;
using MobileGuidingSystem.Model.Data;
using MobileGuidingSystem.View;

namespace MobileGuidingSystem.ViewModel
{
    public class MainModel : Model
    {
        private readonly MapControl _map;
        public Geopoint MyLocation;
        //private readonly bool _onCollisionShow;
        public ObservableCollection<Sight> Sights;
        public static Route CurrentRoute;
        public int Zoomlevel => 17;
        public int DesiredPitch => 45;
        public MapStyle MapStyle = MapStyle.Road;

        private readonly Color _routeColor = Colors.Blue;
        private readonly Color _outlineColor = Colors.LightBlue;
        public IRandomAccessStreamReference iconImage { get; set; }
        public Point Anchor { get; set; }
        public ContentDialog dialog;
        public IList<Geofence> geofences;
        private bool _playerInRoute = false;
        private List<Geopoint> KnownUserPos = new List<Geopoint>();
        private MainPage page;
        private Sight NextSight;
        private bool routeDrawn;
        private List<MapRouteView> WalkedRoutes = new List<MapRouteView>();

        //TODO: Fix this ofzo
        public MainModel(MapControl mapcontrol, Route route, MainPage page)
        {
            _map = mapcontrol;
            routeDrawn = false;
            this.page = page;
            Sights = new ObservableCollection<Sight>();
            CurrentRoute = route;
            User = new User();
            geofences = GeofenceMonitor.Current.Geofences;
            GeofenceMonitor.Current.GeofenceStateChanged += CurrentOnGeofenceStateChanged;
            LoadData();
            MyLocation = new Geopoint(new BasicGeoposition() { Latitude = 51.5860591, Longitude = 4.793500600000016 });
            iconImage = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/home-pin.png"));
            Anchor = new Point(0.5, 1);
            //drawRoute(new Geopoint(new BasicGeoposition() { Latitude = 51.59000, Longitude = 4.781000 }), new Geopoint(new BasicGeoposition(){ Longitude = 4.780172, Latitude = 51.586267}) );
            // _map.ZoomLevelChanged += _map_ZoomLevelChanged;
        }

        private async void CurrentOnGeofenceStateChanged(GeofenceMonitor sender, object args)
        {
            var reports = sender.ReadReports();

            await
                CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.High, agileCallback: (async () =>
                    {
                        foreach (GeofenceStateChangeReport report in reports)
                        {
                            switch (report.NewState)
                            {
                                case GeofenceState.Removed:

                                    break;

                                case GeofenceState.Entered:
                                    Debug.WriteLine("entered");
                                    foreach (MapElement mapElement in _map.MapElements)
                                    {
                                        if (mapElement is MapIcon)
                                        {
                                            MapIcon icon = (MapIcon)mapElement;
                                            if (icon.Title == report.Geofence.Id)
                                            {
                                                Sight s = mapElement.ReadData();

                                                for (int i = CurrentRoute.Sights.IndexOf(s)+1;
                                                    i < CurrentRoute.Sights.Count;
                                                    i++)
                                                {
                                                    Sight sight = CurrentRoute.Sights[i];
                                                    if (sight.Name != "")
                                                    {
                                                        NextSight = sight;
                                                        break;
                                                    }
                                                }

                                                deleteRoutes();
                                                redrawRoute(s.Position, NextSight.Position, true);
                                                ContentDialog1 dialog = new ContentDialog1(s);
                                                var result = await dialog.ShowAsync();

                                                // primary button was clicked
                                                if (result == ContentDialogResult.Primary)
                                                {
                                                    RedrawSight(icon);
                                                    int geo = geofences.IndexOf(report.Geofence);
                                                    geofences.RemoveAt(geo);
                                                    page.Frame.Navigate(typeof(SightPage), s);
                                                    break;

                                                }

                                                if (result == ContentDialogResult.None)
                                                {
                                                    dialog.Hide();

                                                }
                                                break;
                                            }
                                        }
                                    }
                                    break;

                                case GeofenceState.Exited:
                                    Debug.WriteLine("kekekekekekekekekekkeke");
                                    break;
                            }
                        }
                    }
            ));
        }

        private void LoadData()
        {
            if (_map.MapElements.Count > 15)
            {
                _map.MapElements.Clear();
            }
            try
            {
                drawSight(CurrentRoute.Sights);
                DrawRoutes(CurrentRoute.Sights);
               // redrawRoute(CurrentRoute.Sights[0], CurrentRoute.Sights[1], false);
            }
            catch { }
        }

        private async void redrawRoute(Geopoint p1, Geopoint p2, bool deleteprevious)
        {
            if (deleteprevious)
            {
                try
                {
                    _map.Routes.RemoveAt(_map.Routes.Count - 1);

                }
                catch { }
            }

            MapRouteFinderResult routeFinderResult = await MapRouteFinder.GetWalkingRouteAsync(p1, p2);

            if (routeFinderResult.Status == MapRouteFinderStatus.Success)
            {
                MapRouteView routeView = new MapRouteView(routeFinderResult.Route);
                routeView.RouteColor = Colors.Green;
                _map.Routes.Add(routeView);

            }
        }

        private void RedrawSight(MapIcon icon)
        {

            foreach (MapElement mapElement in _map.MapElements)
            {
                if (mapElement.GetType() != typeof(MapIcon)) continue;
                if (mapElement.Equals(icon))
                {
                    int index = _map.MapElements.IndexOf(mapElement);
                    _map.MapElements.RemoveAt(index);
                    RandomAccessStreamReference image;
                    Sight s = icon.ReadData();
                    if (s.Name != "VVV")
                    {
                         image =
                            RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/entered-pin.png"));
                    }
                    else
                    {
                         image =
                            RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/home-entered-pin.png"));
                    }

                    var ancherpoint = new Point(0.5, 1);
                    var seenSight = new MapIcon
                    {
                        Title = s.Name,
                        Location = s.Position,
                        NormalizedAnchorPoint = ancherpoint,
                        Image = image,
                        ZIndex = 4,
                        CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible

                    };
                    seenSight.AddData(s);
                    _map.MapElements.Add(seenSight);
                    break;
                }
            }
        }

        public async void DrawRoute(Geopoint p1, Geopoint p2)
        {
            MapRouteFinderResult routeFinderResult = await MapRouteFinder.GetWalkingRouteAsync(p1, p2);

            if (routeFinderResult.Status == MapRouteFinderStatus.Success)
            {
                deleteRoutes();
                MapRouteView routeView = new MapRouteView(routeFinderResult.Route);
                routeView.RouteColor = _routeColor;
                routeView.OutlineColor = _outlineColor;
                _map.Routes.Add(routeView);

            }
        }

        private void deleteRoutes()
        {
            for (int i = _map.Routes.Count - 1; i >= 0; i--)
            {
                if (_map.Routes[i].RouteColor == Colors.Green)
                    _map.Routes.RemoveAt(i);

            }
        }

        //private void deleteRouteswalked()
        //{
        //    for (int i = _map.Routes.Count - 1; i >= 0; i--)
        //    {
        //        if (_map.Routes[i].RouteColor == Colors.Red)
        //        {
        //            _map.Routes.RemoveAt(i);
        //        }
        //    }
        //}

        //private void updateRoute()
        //{
        //    Debug.WriteLine(KnownUserPos.Count);
        //    for (int i = 0; i < _map.Routes.Count; i++)
        //    {
        //        if (_map.Routes[i].RouteColor == Colors.Red)
        //        {
        //            if (KnownUserPos.Count > 10)
        //            {
        //                for (int j = KnownUserPos.Count - 1; j >= 0; j--)
        //                {
        //                    int jj = j % 2;
        //                    if (jj == 1)
        //                    {
        //                        KnownUserPos.RemoveAt(j);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //}

        public async void DrawRoutes(List<Geopoint> positions)
        {
            MapRouteFinderResult routeFinderResult = await MapRouteFinder.GetWalkingRouteFromWaypointsAsync(positions);

            if (routeFinderResult.Status == MapRouteFinderStatus.Success)
            {
                deleteRoutes();
                MapRouteView routeView = new MapRouteView(routeFinderResult.Route);
                routeView.RouteColor = _routeColor;
                routeView.OutlineColor = _outlineColor;
                _map.Routes.Add(routeView);
            }
            else if (routeFinderResult.Status == MapRouteFinderStatus.NoRouteFoundWithGivenOptions)
            {
                deleteRoutes();
                MapRouteView routeView = new MapRouteView(routeFinderResult.Route);
                routeView.RouteColor = _routeColor;
                routeView.OutlineColor = _outlineColor;
                _map.Routes.Add(routeView);
            }

        }

        public async void DrawWalkedRoute(List<Geopoint> list)
        {
            deleteWalkedroute();
            if (list.Count > 2)
            {
                MapRouteFinderResult routeFinder =
                    await MapRouteFinder.GetWalkingRouteAsync(list[list.Count - 2], list[list.Count - 1]);
                if (routeFinder.Status == MapRouteFinderStatus.Success)
                {
                    MapRouteView routeView = new MapRouteView(routeFinder.Route);
                    routeView.RouteColor = Colors.Red;
                    WalkedRoutes.Add(routeView);
                   _map.Routes.Add(routeView);
                }
                else if (routeFinder.Status == MapRouteFinderStatus.NoRouteFoundWithGivenOptions)
                {
                    MapRouteView routeView = new MapRouteView(routeFinder.Route);
                    routeView.RouteColor = Colors.Red;
                    WalkedRoutes.Add(routeView);
                    _map.Routes.Add(routeView);
                }
            }
        }

        private void deleteWalkedroute()
        {
            if(WalkedRoutes.Count > 40)
            {
                for(int i = 0; i < 3; i++)
                {
                    try
                    {
                    if (_map.Routes[i].RouteColor.Equals(Colors.Red))
                        {
                            _map.Routes.RemoveAt(i);
                        }
                    }
                    catch { }
                }
            }
        }

        public void DrawRoutes(List<Sight> sightlist)
        {
            List<Geopoint> positions = new List<Geopoint>();

            if (User.Location != null)
            {
                positions.Add(User.Location);
            }

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


        public void drawSight(List<Sight> list)
        {
            var ancherpoint = new Point(0.5, 1);
            RandomAccessStreamReference image;


            foreach (Sight sight in list)
            {
                MapIcon sightIcon;
                if (sight.Name == "VVV"){ image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/home-pin.png")); }
                else if(sight.Name == ""){ image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/TransparentWayPoint.png")); }
                else{ image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/pin.png")); }

                         sightIcon = new MapIcon
                        {
                            Title = sight.Name,
                            Location = sight.Position,
                            NormalizedAnchorPoint = ancherpoint,
                            Image = image,
                            ZIndex = 4,
                            CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible
                        };
                        sightIcon.AddData(sight);
                    
                    _map.MapElements.Add(sightIcon);
                    AddGeofence(sight.Position, sight.Name, 20);
                    //_map.MapElements.Add(GetCircleMapPolygon(sight.Position.Position, 20));
            }
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
            KnownUserPos.Add(User.Location);
            DrawWalkedRoute(KnownUserPos);
            _map.MapElements.Add(player);

            if (_playerInRoute == false)
            {
                DrawRoutes(CurrentRoute.Sights);
                _playerInRoute = true;
            }
        }

        public void UpdatePos()
        {
            if (_map.MapElements != null)
            {
                foreach (MapElement element in _map.MapElements)
                {
                    if (element is MapIcon)
                    {
                        MapIcon player = (MapIcon)element;
                        if (player.ZIndex == 13)
                        {
                            int index = _map.MapElements.IndexOf(player);
                            _map.MapElements.RemoveAt(index);
                            _map.Center = User.Location;
                            break;
                        }
                    }

                }
            }
        }

        private void AddGeofence(Geopoint location, string title, double radius)
        {
            string fenceKey = title;
            // the geofence is a circular region:
            Geocircle geocircle = new Geocircle(location.Position, radius);

            try
            {
                geofences.Add(new Geofence(fenceKey, geocircle, MonitoredGeofenceStates.Entered, false, TimeSpan.FromSeconds(0.1)));

            }
            catch (Exception e)
            {
                Debug.WriteLine("catched" + e);
            }

        }

        public MapPolygon GetCircleMapPolygon(BasicGeoposition originalLocation, double radius)
        {
            MapPolygon retVal = new MapPolygon();

            List<BasicGeoposition> locations = new List<BasicGeoposition>();
            double latitude = originalLocation.Latitude * Math.PI / 180.0;
            double longitude = originalLocation.Longitude * Math.PI / 180.0;
            // double x = radius / 3956; // Miles
            double x = radius / 6371000; // Meters 
            for (int i = 0; i <= 360; i += 10) // <-- you can modify this incremental to adjust the polygon.
            {
                double aRads = i * Math.PI / 180.0;
                double latRadians = Math.Asin(Math.Sin(latitude) * Math.Cos(x) + Math.Cos(latitude) * Math.Sin(x) * Math.Cos(aRads));
                double lngRadians = longitude + Math.Atan2(Math.Sin(aRads) * Math.Sin(x) * Math.Cos(latitude), Math.Cos(x) - Math.Sin(latitude) * Math.Sin(latRadians));

                BasicGeoposition loc = new BasicGeoposition() { Latitude = 180.0 * latRadians / Math.PI, Longitude = 180.0 * lngRadians / Math.PI };
                locations.Add(loc);
            }

            retVal.Path = new Geopath(locations);
            retVal.FillColor = Windows.UI.Color.FromArgb(0x40, 0x10, 0x10, 0x80);
            retVal.StrokeColor = Windows.UI.Colors.Blue;
            retVal.StrokeThickness = 2;

            return retVal;
        }

    }


}