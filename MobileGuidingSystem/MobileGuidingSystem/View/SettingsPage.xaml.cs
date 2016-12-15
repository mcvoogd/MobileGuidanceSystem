using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileGuidingSystem.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            MenuListBox.Background = new SolidColorBrush(Color.FromArgb(150,255,255,255));
            Frame.Navigate(typeof(MainPage));
            Map.IsSelected = true;

        }

        private void HamburgerButton_OnClick(object sender, RoutedEventArgs e)
        {          
            HamburgerSplitview.IsPaneOpen = !HamburgerSplitview.IsPaneOpen;
        }

        //TODO add navigation to the pages
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
             if (Map.IsSelected)
             {
                 Frame.Navigate(typeof(MainPage));
             }
            else if (RouteSelection.IsSelected)
             {
                 Frame.Navigate(typeof(RouteSelectionPage));
             }
            else if (Sights.IsSelected)
             {
                 Frame.Navigate(typeof(SightPage));
             }
            else if (Language.IsSelected)
             {
                 Frame.Navigate(typeof(LanguageSelectionPage));
             }
            else if (Settings.IsSelected)
             {
                 Frame.Navigate(typeof(SettingsPage));
             }
            else if (Help.IsSelected)
            {
                Frame.Navigate(typeof(HelpPage));
            }
        }
    }
}
