using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using MobileGuidingSystem.Model.Data;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileGuidingSystem.View
{

    public sealed partial class ContentDialog1 : ContentDialog
    {
        public ISight sight;
        public double height;

        public string imagepath
        {
            get { return "Assets/" + sight.ImagePaths[0]; }
        }

        public ContentDialog1()
        {
            this.InitializeComponent();
            DataContext = sight;
            height = ApplicationView.GetForCurrentView().VisibleBounds.Height;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Window.Current.Content = new SightPage();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
    }
}
