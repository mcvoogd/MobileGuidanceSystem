﻿using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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
            Frame.Navigate(typeof(LanguageSelectionPage));
            Map.IsSelected = true;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
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
    }
}
