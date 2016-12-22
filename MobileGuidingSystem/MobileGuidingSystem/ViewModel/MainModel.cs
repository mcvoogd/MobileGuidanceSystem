using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
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


        //TODO: Fix this ofzo
        public MainModel(MapControl mapcontrol)
        {
            _map = mapcontrol;
            Sights = new ObservableCollection<Sight>();
            LoadData();
            MyLocation = new Geopoint(new BasicGeoposition() { Latitude = 51.5860591, Longitude = 4.793500600000016 });
            User = new User();


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

        private void LoadData()
        {
            //Sights.Add();
            //drawSight(RouteLoader.Sights);

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

        public void DrawRoutes(List<Sight> sightlist)
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

        //public void drawSight(Geopoint position, String title)
        public void drawSight(List<Sight> list)
        {
            var ancherpoint = new Point(0.5, 1);
            var image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/home-pin.png"));
            foreach (Sight sight in list)
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
                        MapIcon player = (MapIcon)element;
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
           // throw new NotImplementedException();
           sender.Hide();
        }

        private void Dialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // throw new NotImplementedException();
            ContentDialog1 dial = (ContentDialog1) sender;
            Window.Current.Content = new SightPage(dial.sight);
        }
    }


}