using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
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
        public MainModel Model;
        Geolocator _geolocator;
        private bool _positionSet = false;
       // const int Maxzoom = 17;
        public static bool isLoaded = false;

        public static NavigationCacheMode mode;
        // Property.
        public NavigationCacheMode Mode
        {
            get { return mode; }
            set
            {
                if (value != mode)
                {
                    mode = value;
                    // Notify of the change.
                    NotifyPropertyChanged();
                }
            }
        }


        public MainPage()
        {
            this.InitializeComponent();
            if (mode != NavigationCacheMode.Required)
            {
                Mode = NavigationCacheMode.Required;

            }
            this.Loaded += PageLoaded;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!isLoaded)
            {
                Model = new MainModel(MyMap, (Route) e.Parameter, this);
                DataContext = Model;
                isLoaded = true;
            }
        }

        //private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var b = (StackPanel) sender;
        //    var selected = (Sight) b.DataContext;
        //    MyMap.Center = selected.Position;

        //    Popup popup = new Popup();
        //    popup.DataContext = selected;
        //}

        private async void PageLoaded(Object sender, RoutedEventArgs e)
        {
            GeolocationAccessStatus accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    _geolocator = new Geolocator { DesiredAccuracy = PositionAccuracy.Default, MovementThreshold = 2 };
                    // geolocator = new Geolocator {ReportInterval = 1000};
                    _geolocator.PositionChanged += Geolocator_PositionChanged;

                    Geoposition pos = await _geolocator.GetGeopositionAsync();
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        Model.DrawPlayer(pos);
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
                Model.DrawPlayer(args.Position);
            });

            _positionSet = true;
        }
            

        //private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        //{
        //    var b = (StackPanel)sender;
        //    var selected = (Sight)b.DataContext;
        //    MyMap.Center = selected.Position;
        //    MyMap.Center = selected.Position;

        //    var FlyOut = new Flyout();
        //    var grid = new Grid();
        //    var stackpanel = new StackPanel() { DataContext = selected };
        //    stackpanel.Tapped += StackpanelOnTapped;
        //    grid.Children.Add(stackpanel);
        //    stackpanel.Children.Add(new TextBlock() { Text = selected.Name });
        //    stackpanel.Children.Add(new Image() { Source = new BitmapImage(new Uri("ms-appx:///Assets/avans_logo.png")), MaxHeight = 40.0, MaxWidth = 40.0 });
        //    FlyOut.Content = grid;
        //    FlyOut.ShowAt(sender as FrameworkElement);
        //}

        //private void StackpanelOnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        //{
        //    var b = (StackPanel)sender;
        //    var selected = (Sight)b.DataContext;
        //    Frame.Navigate(typeof(SightPage), selected);
        //}

        private void NavToSight(Sight sight)
        {
            Frame.Navigate(typeof(SightPage), sight);
        }

        private async void MyMap_OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            //Model.myMap_OnMapElementClick(sender , args);
            MapIcon myClickedIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;

            Sight clickedSight = myClickedIcon.ReadData();
            ContentDialog1 dialog = new ContentDialog1(clickedSight);
            var result = await dialog.ShowAsync();

            // primary button was clicked
            if (result == ContentDialogResult.Primary)
            {
                NavToSight(clickedSight);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // PropertyChanged event triggering method.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
