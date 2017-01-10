using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;
using MobileGuidingSystem.Model.Data;

namespace MobileGuidingSystem.ViewModel
{
    public static class MapElementData
    {
        #region Attached Dependency Property ObjectData
        public static readonly DependencyProperty ObjectDataProperty =
             DependencyProperty.RegisterAttached("ObjectData",
             typeof(Sight),
             typeof(MapElementData),
             new PropertyMetadata(default(object), null));

        public static object GetObjectData(MapElement obj)
        {
            return obj.GetValue(ObjectDataProperty);
        }

        public static void SetObjectData(
           DependencyObject obj,
           object value)
        {
            obj.SetValue(ObjectDataProperty, value);
        }
        #endregion

        public static void AddData(this MapElement element, object data)
        {
            SetObjectData(element, data);
        }
        
        public static Sight ReadData(this MapElement element)
        {
            return (Sight)GetObjectData(element);
        }
    }
}