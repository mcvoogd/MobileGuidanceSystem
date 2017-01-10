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
using MobileGuidingSystem.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MobileGuidingSystem.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SightPage : Page
    {
        public Sight sight;
        //public List<string> imagePaths;

        public SightPage()
        {
            //imagePaths = new List<string>();
            this.InitializeComponent();
            
        }  

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            sight = (Sight) e.Parameter;
            DataContext = sight;

            AddToSplitView();
        }

        //public void ConvertImagePaths()
        //{
        //    foreach (string s in sight.ImagePaths)
        //    {
        //        imagePaths.Add("ms-appx:///Assets/Pictures/" + s);
        //    }
        //}
                
        public void AddToSplitView()
        {
            if (sight.Name == "Grote Kerk")
            {
                MediaElement media = new MediaElement();
                media.Source = new Uri("ms-appx:///Assets/audio_grote_klok.mp3");
                media.AutoPlay = false;
                media.AreTransportControlsEnabled = true;
                flipView.Items.Add(media);
            }
            foreach (string s in sight.FullImagePaths)
            {
                ImageBrush brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri(s));
                brush.Stretch = Stretch.UniformToFill;
                FlipViewItem item = new FlipViewItem();
                item.Background = brush;
                flipView.Items.Add(item);
                
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), MainModel.CurrentRoute);
        }
    }
}
