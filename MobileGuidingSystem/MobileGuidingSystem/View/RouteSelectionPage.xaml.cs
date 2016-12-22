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

        ObservableCollection<ISightProvider> _routes = new ObservableCollection<ISightProvider>();
        public RouteSelectionPage()
        {
            this.InitializeComponent();
            _routes.Add(new BlindwallsDatabase());
        }

        
    }
}
