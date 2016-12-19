﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls.Maps;
using MobileGuidingSystem.Model;
using MobileGuidingSystem.Model.Data;

namespace MobileGuidingSystem.ViewModel
{
    public class MainModel : Model
    {
        private readonly MapControl _map;
        public Geopoint myLocation;
        private readonly bool onCollisionShow;
        public ObservableCollection<ISight> sights;

        public int zoomlevel
        {
            get { return 17; }
        }

        public int DesiredPitch
        {
            get { return 45; }
        }

        public MapStyle MapStyle = MapStyle.Road;

        private readonly Color RouteColor = Colors.Blue;
        private readonly Color OutlineColor = Colors.LightBlue;

        public MainModel(MapControl mapcontrol)
        {
            _map = mapcontrol;
            sights = new ObservableCollection<ISight>();
            LoadData();
            DrawRoutes(sights);
            myLocation = new Geopoint(new BasicGeoposition() {Latitude = 51.5860591, Longitude = 4.793500600000016});
            user = new User();
            //drawRoute(new Geopoint(new BasicGeoposition() { Latitude = 51.59000, Longitude = 4.781000 }), new Geopoint(new BasicGeoposition(){ Longitude = 4.780172, Latitude = 51.586267}) );

        }

        private void LoadData()
        {
            BlindwallsDatabase.Sights.ForEach(s=>sights.Add(s));
        }

        private void _map_ZoomLevelChanged(MapControl sender, object args)
        {
            if (_map.ZoomLevel < 17)
            {
                _map.ZoomLevel = 17;
            }
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

        public void DrawRoutes(ObservableCollection<ISight> sightlist)
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

        public void DrawPlayer(Geoposition position)
        {

            var ancherpoint = new Point(0.5, 0.5);
            var image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/PlayerPin.png"));
            var Player = new MapIcon
            {
                Title = "",
                Location = position.Coordinate.Point,
                NormalizedAnchorPoint = ancherpoint,
                Image = image,
                ZIndex = 13
            };
            updatePos();
            _map.MapElements.Add(Player);
        }

        public void updatePos()
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