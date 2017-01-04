using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileGuidingSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationPage : Page
    {
        public NavigationPage()
        {
            this.InitializeComponent();
            MenuListBox.Background = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255));
            HamburgerButton.Visibility = Visibility.Collapsed;
            Frame.Navigate(typeof(LanguageSelectionPage));
            Map.IsSelected = true;

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    a.Handled = true;
                }
            };
        }

        private void HamburgerButton_OnClick(object sender, RoutedEventArgs e)
        {
            HamburgerSplitview.IsPaneOpen = !HamburgerSplitview.IsPaneOpen;
        }

        //TODO add navigation to the pages
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HamburgerSplitview.DisplayMode = SplitViewDisplayMode.Overlay;
            HamburgerButton.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Collapsed;
            if (Menu.IsSelected)
            {
                HamburgerSplitview.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                HamburgerButton.Visibility = Visibility.Collapsed;
                Menu.Visibility = Visibility.Visible;
            }
            if (Map.IsSelected)
            {
                Frame.Navigate(typeof(MainPage));
                HamburgerSplitview.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                HamburgerButton.Visibility = Visibility.Collapsed;
                Menu.Visibility = Visibility.Visible;
            }
            else if (RouteSelection.IsSelected)
            {
                Frame.Navigate(typeof(RouteSelectionPage));
            }
            else if (Sights.IsSelected)
            {
                Frame.Navigate(typeof(SightsPage));
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
            HamburgerSplitview.IsPaneOpen = false;
        }

        private void HamburgerSplitview_OnPaneClosed(SplitView sender, object args)
        {
            if (!Menu.IsSelected)
            {
                if(!Map.IsSelected)
                    HamburgerButton.Visibility = Visibility.Visible;
            }
        }

    }
}
