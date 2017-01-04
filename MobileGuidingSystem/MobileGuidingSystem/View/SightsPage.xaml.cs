using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MobileGuidingSystem.Model.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileGuidingSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SightsPage : Page
    {
        private Route route;
        public SightsPage()
        {
            this.InitializeComponent();
            route = Route.Routes[0];
            SightList.ItemsSource = route.Sights;
        }

        private void SightList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = (ListView) sender;
            Sight s = (Sight) lv.SelectedItem;
            Frame.Navigate(typeof(SightPage), s);
        }
    }
}