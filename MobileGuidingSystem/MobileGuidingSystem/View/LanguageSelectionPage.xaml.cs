using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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

        private void Dutch_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ApplicationLanguages.PrimaryLanguageOverride = "nl-NL";
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
            Windows.ApplicationModel.Resources.Core.ResourceManager.Current.DefaultContext.Reset();
            Frame.Navigate(this.GetType());
        }

        private void English_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ApplicationLanguages.PrimaryLanguageOverride = "en-US";
            Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();
            Windows.ApplicationModel.Resources.Core.ResourceManager.Current.DefaultContext.Reset();
            Frame.Navigate(this.GetType());
        }
    }
}
