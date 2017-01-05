using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MobileGuidingSystem.Model.Data;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileGuidingSystem.View
{

    public sealed partial class ContentDialog1 : ContentDialog
    {
        public Sight sight { get; set; }
        public string imagepath
        {
            get { return returnImagePath(); }
            //get { return "ms-appx:///Assets/avans_logo.png"; }
             // get { return "ms-appx:///Assets/Pictures/bwg_damienpoulain_rosameininger-1599.jpg"; }
        }

        public string returnImagePath()
        {
            if (sight.ImagePaths.Count != 0)
            {
               return "ms-appx:///Assets/Pictures/" + sight.ImagePaths[0];
            }

            return "ms-appx:///Assets/NoImage.png";
        }

        public ContentDialog1(Sight sight)
        {
            this.InitializeComponent();
            this.sight = sight;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
    }
}
