﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using MobileGuidingSystem.Model.Data;
using MobileGuidingSystem.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MobileGuidingSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainModel model;
        Geolocator geolocator;
        private bool positionSet = false;
        const int maxzoom = 17;


        public MainPage()
        {
            this.InitializeComponent();
            model = new MainModel(MyMap);
            this.Loaded += pageLoaded;
            DataContext = model;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var b = (StackPanel) sender;
            var selected = (ISight) b.DataContext;
            MyMap.Center = selected.Position;

            Popup popup = new Popup();
            popup.DataContext = selected;
        }

        private async void pageLoaded(Object sender, RoutedEventArgs e)
        {
            GeolocationAccessStatus accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    geolocator = new Geolocator {DesiredAccuracy = PositionAccuracy.Default, MovementThreshold = 1};
                   // geolocator = new Geolocator {ReportInterval = 1000};
                    geolocator.PositionChanged += Geolocator_PositionChanged;

                    Geoposition pos = await geolocator.GetGeopositionAsync();
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        model.DrawPlayer(pos);
                    });
                    break;
                case GeolocationAccessStatus.Denied:
                    break;

                case GeolocationAccessStatus.Unspecified:
                    break;
            }
        }

        private async void Geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
               // model.centerMap(args.Position);
                model.DrawPlayer(args.Position);
            });

            positionSet = true;
        }
    }
}
