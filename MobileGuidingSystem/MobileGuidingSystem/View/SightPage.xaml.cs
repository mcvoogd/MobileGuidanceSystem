using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using MobileGuidingSystem.Model.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileGuidingSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SightPage : Page
    {
        public ISight sight;
        //public List<string> imagePaths;

        public SightPage(ISight sight)
        {
            this.sight = sight;
            //imagePaths = new List<string>();
            this.InitializeComponent();
            AddToSplitView();
        }  

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ISight sight = (ISight) e.Parameter;
            DataContext = sight;
        }

        //public void ConvertImagePaths()
        //{
        //    foreach (string s in sight.ImagePaths)
        //    {
        //        imagePaths.Add("ms-appx:///Assets/Pictures/" + s);
        //    }
        //}

        public async void AddToSplitView()
        { 
            foreach (string s in sight.ImagePaths)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Pictures/" + s));
                FlipViewItem item = new FlipViewItem();
                item.Background = brush;
                FlipView.Items.Add(item);
            }
             

        }
    }
}
