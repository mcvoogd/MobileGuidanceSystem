using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
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
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Route route = comboBox.SelectionBoxItem as Route;
            Frame.Navigate(typeof(MainPage), route);
        }
    }
}
