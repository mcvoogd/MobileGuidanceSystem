using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace MobileGuidingSystem.Model.Data
{
    public class Sight
    {
        public string IconPath;
        public string Name;
        public Geopoint Position;
        public RandomAccessStreamReference Image;
        public Point NormalizedAnchorPoint;

        public Sight(string name, string iconPath, Geopoint position)
        {
            this.Name = name;
            this.IconPath = iconPath;
            this.Position = position;
            Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/" + iconPath));
            NormalizedAnchorPoint = new Point(1,1);
        }
    }
}