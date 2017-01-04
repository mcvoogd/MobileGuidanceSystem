using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
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
        private int selectedRoute;
        private bool _baseRoute = true;
        private List<Geopoint> KnownUserPos = new List<Geopoint>();


        //TODO: Fix this ofzo
        public MainModel(MapControl mapcontrol)
        {
            _map = mapcontrol;
            Sights = new ObservableCollection<Sight>();
            LoadData();
            MyLocation = new Geopoint(new BasicGeoposition() { Latitude = 51.5860591, Longitude = 4.793500600000016 });
            User = new User();
            geofences = GeofenceMonitor.Current.Geofences;
            GeofenceMonitor.Current.GeofenceStateChanged += CurrentOnGeofenceStateChanged;
            //DrawRoutes(RouteLoader.Sights);

            iconImage = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/home-pin.png"));
            Anchor = new Point(0.5, 1);
            dialog = new ContentDialog();
            dialog.PrimaryButtonClick += Dialog_PrimaryButtonClick;
            dialog.SecondaryButtonClick += Dialog_SecondaryButtonClick;
            dialog.Hide();
            //drawRoute(new Geopoint(new BasicGeoposition() { Latitude = 51.59000, Longitude = 4.781000 }), new Geopoint(new BasicGeoposition(){ Longitude = 4.780172, Latitude = 51.586267}) );
            //  _map.ZoomLevelChanged += _map_ZoomLevelChanged;
        }


        private async void CurrentOnGeofenceStateChanged(GeofenceMonitor sender, object args)
        {
            var reports = sender.ReadReports();

            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, (() =>
            {
                foreach (GeofenceStateChangeReport report in reports)
                {
                switch (report.NewState)
                {
                    case GeofenceState.Removed:

                        break;

                    case GeofenceState.Entered:
                        foreach (MapElement mapElement in _map.MapElements)
                        {
                            if (mapElement is MapIcon)
                            {
                                MapIcon icon = (MapIcon) mapElement;
                                if (icon.Title == report.Geofence.Id)
                                {
                                    icon.Image =
                                        RandomAccessStreamReference.CreateFromUri(
                                            new Uri("ms-appx:///Assets/entered-pin.png"));
                                    _map.MapElements.RemoveAt(_map.MapElements.IndexOf(icon));
                                    _map.MapElements.Add(icon);
                                }
                            }
                        }
                        break;

                    case GeofenceState.Exited:

                        break;
                }
                }
            }
            ));
        }

        private void LoadData()
        {
            //Sights.Add();
            drawSight(Route.Routes[0].Sights);
            DrawRoutes(Route.Routes[0].Sights);
            selectedRoute = 0;

        }

        public async void DrawRoute(Geopoint p1, Geopoint p2)
        {
            MapRouteFinderResult routeFinderResult = await MapRouteFinder.GetWalkingRouteAsync(p1, p2);

            if (routeFinderResult.Status == MapRouteFinderStatus.Success)
            {
                deleteRoutes();              
                if (_baseRoute == false)
                {
                    MapRouteView routeView = new MapRouteView(routeFinderResult.Route);
                    routeView.RouteColor = _routeColor;
                    routeView.OutlineColor = _outlineColor;
                    _map.Routes.Add(routeView);
                }
            }
        }

        private void deleteRoutes()
        {
            for (int i = _map.Routes.Count - 2; i >= 0; i--)
            {
                if(_map.Routes[i].RouteColor == _routeColor)
                _map.Routes.RemoveAt(i);      
            }    
        }

        private void deleteRouteswalked()
        {
            for (int i = _map.Routes.Count - 2; i >= 0; i--)
            {
                if (_map.Routes[i].RouteColor == Colors.Red)
                    _map.Routes.RemoveAt(i);
            }
        }

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
            if (list.Count > 2)
            {
                list.RemoveAt(1);
            }
            if (list.Count == 2)
                {
                   MapRouteFinderResult routeFinder =
                        await MapRouteFinder.GetWalkingRouteAsync(list[0], list[list.Count - 1]);
                    if (routeFinder.Status == MapRouteFinderStatus.Success)
                    {
                        deleteRouteswalked();
                        MapRouteView routeView = new MapRouteView(routeFinder.Route);
                        routeView.RouteColor = Colors.Red;
                        routeView.OutlineColor = _outlineColor;
                        _map.Routes.Add(routeView);
                    }
                    else if (routeFinder.Status == MapRouteFinderStatus.NoRouteFoundWithGivenOptions)
                    {
                        deleteRouteswalked();
                        MapRouteView routeView = new MapRouteView(routeFinder.Route);
                        routeView.RouteColor = Colors.Red;
                        routeView.OutlineColor = _outlineColor;

                        _map.Routes.Add(routeView);
                    }
                }
            }

        public void DrawRoutes(List<Sight> sightlist)
        {
            List<Geopoint> positions = new List<Geopoint>();

            if (User.Location != null)
            {
                positions.Add( User.Location);
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
            var image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/home-pin.png"));
            var image2 = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/TransparentWayPoint.png"));
            foreach (Sight sight in list)
            {

                if (sight.Name != "")
                {
                    var Sight = new MapIcon
                    {
                        Title = sight.Name,
                        Location = sight.Position,
                        NormalizedAnchorPoint = ancherpoint,
                        Image = image,
                        ZIndex = 4,
                        CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible
                       
                };
                    Sight.AddData(sight);
                    _map.MapElements.Add(Sight);
                }
                else
                {
                    var SightWIthoutPin = new MapIcon
                    {
                        Title = sight.Name,
                        Location = sight.Position,
                        NormalizedAnchorPoint = ancherpoint,
                        Image = image2,
                        ZIndex = 4,
                        CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible

                    };
                    SightWIthoutPin.AddData(SightWIthoutPin);
                    _map.MapElements.Add(SightWIthoutPin);
                }
                
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
            DrawRoutes(Route.Routes[selectedRoute].Sights);
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

        public async void myMap_OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            MapIcon myClickedIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;

            Sight clickedSight = myClickedIcon.ReadData();
            ScrollViewer SV = new ScrollViewer();
            TextBlock txtBlock = new TextBlock();


            txtBlock.Text = clickedSight.Address + "\r" + clickedSight.Description + "\r";
            txtBlock.TextWrapping = TextWrapping.Wrap;
            SV.Content = txtBlock;
            SV.VerticalAlignment = VerticalAlignment.Stretch;

            //var image =
            //    RandomAccessStreamReference.CreateFromUri(
            //        new Uri("ms-appx:///Assets/Pictures/" + clickedSight.ImagePaths[0]));

            //Image image = new Image();

            //image.Source = clickedSight.ImageStreamReferences;

            //dialog.Content = image;
            //dialog.Title = clickedSight.Name;
            //dialog.PrimaryButtonText = "visit " + clickedSight.Name;
            //dialog.SecondaryButtonText = "Close";

            //await dialog.ShowAsync();

            ContentDialog1 dialog1 = new ContentDialog1(clickedSight);
            await dialog1.ShowAsync();
        }

        private void Dialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            sender.Hide();
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ContentDialog1 dial = (ContentDialog1)sender;
            Window.Current.Content = new SightPage(dial.sight);
        }

        private void AddGeofence(Geopoint location, string title, double radius)
        {
            string fenceKey = title;


            // the geofence is a circular region:
            Geocircle geocircle = new Geocircle(location.Position, radius);

            bool singleUse = false;

            MonitoredGeofenceStates mask = MonitoredGeofenceStates.Entered | MonitoredGeofenceStates.Exited;

            TimeSpan dwellTime = new TimeSpan(0,1,0);
            TimeSpan duration = new TimeSpan(0,1,0);

            DateTimeOffset startDateTime = new DateTimeOffset(0,0,0,0,1,0,new TimeSpan(0,0,1,0));

            geofences.Add(new Geofence(fenceKey, geocircle, mask, singleUse, dwellTime, startDateTime, duration));
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