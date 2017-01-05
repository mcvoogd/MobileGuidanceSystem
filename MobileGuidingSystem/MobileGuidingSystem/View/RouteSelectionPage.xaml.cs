using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MobileGuidingSystem.Model.Data;
using MobileGuidingSystem.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileGuidingSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RouteSelectionPage : Page
    {
        private readonly ObservableCollection<Route> Routes = new ObservableCollection<Route>(Route.Routes);
        public RouteSelectionPage()
        {
            this.InitializeComponent();
            comboBox.SelectionChanged += ComboBoxOnSelectionChanged;
            comboBox.SelectedItem = Routes[0];
        }

        private void ComboBoxOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        { 
            ComboBox cb = (ComboBox) sender;
            Route route = (Route) cb.SelectedItem;

            if (route.Name == "Historische Kilometer")
            {
                Description.Text = "In de Historische Kilometer stelt de VVV Breda het oudste en mooiste gedeelte van de stad Breda aan u voor.";
            }
            else
            {
                Description.Text =
                    "De Blind Walls Gallery werkt aan een nieuw stadsgezicht. Vanaf 2015 verschijnen zowel op tijdelijke als permanente locaties muurschilderingen gemaakt door internationale talenten op het gebied van grafisch ontwerp, street art, typografie en illustratie.";
            }
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MainPage.isLoaded = false;
            MainPage.mode = NavigationCacheMode.Disabled;
            Route route = comboBox.SelectionBoxItem as Route;
            Frame.Navigate(typeof(MainPage), route);
        }
    }
}
