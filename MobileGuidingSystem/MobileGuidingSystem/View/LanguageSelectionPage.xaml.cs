using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using MobileGuidingSystem.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileGuidingSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LanguageSelectionPage : Page
    {
        public LanguageSelectionPage()
        {
            this.InitializeComponent();
        }

        private async void Dutch_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ApplicationLanguages.PrimaryLanguageOverride = "nl-NL";
            await Task.Delay(10);
            if (MainModel.CurrentRoute != null)
                Frame.Navigate(typeof(MainPage), MainModel.CurrentRoute);
            else
            {
                Frame.Navigate(typeof(RouteSelectionPage));
            }
            
        }

        private async void English_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ApplicationLanguages.PrimaryLanguageOverride = "en-US";
            await Task.Delay(10);
            if (MainModel.CurrentRoute != null)
                Frame.Navigate(typeof(MainPage), MainModel.CurrentRoute);
            else
            {
                Frame.Navigate(typeof(RouteSelectionPage));
            }
        }
    }
}
