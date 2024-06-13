using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SchoolGradingSystem


{
    public static class ButtonHelper
    {
        // Define a DependencyProperty called HoverImageProperty.
        // This is the actual attached property that will hold the hover image source.
        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.RegisterAttached(
                "HoverImage",                // The name of the attached property
                typeof(string),             // The type of the property (in this case, string for the image source)
                typeof(ButtonHelper),      // The type that owns the attached property
                new PropertyMetadata(default(string)));   // Metadata, including a default value

        // Define the setter method for the attached property.
        // This method is called to set the value of the attached property on a UI element.
        public static void SetHoverImage(UIElement element, string value)
        {
            element.SetValue(HoverImageProperty, value);
        }

        // Define the getter method for the attached property.
        // This method is called to get the value of the attached property from a UI element.
        public static string GetHoverImage(UIElement element)
        {
            return (string)element.GetValue(HoverImageProperty);
        }
    }

    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path)
            {
                return new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class ButtonHelper2
    {
        // Define a DependencyProperty called HoverImageProperty.
        // This is the actual attached property that will hold the hover image source.
        public static readonly DependencyProperty HoverImageProperty =
            DependencyProperty.RegisterAttached(
                "HoverImage2",                // The name of the attached property
                typeof(ImageSource),             // The type of the property (in this case, string for the image source)
                typeof(ButtonHelper),      // The type that owns the attached property
                new PropertyMetadata(default(ImageSource)));   // Metadata, including a default value

        // Define the setter method for the attached property.
        // This method is called to set the value of the attached property on a UI element.
        public static void SetHoverImage2(UIElement element, string value)
        {
            element.SetValue(HoverImageProperty, value);
        }

        // Define the getter method for the attached property.
        // This method is called to get the value of the attached property from a UI element.
        public static string GetHoverImage2(UIElement element)
        {
            return (string)element.GetValue(HoverImageProperty);
        }
    }

    public static class ObjectDescription
    {
        // Define an attached property called "elementString".
        public static readonly DependencyProperty ElementStringProperty =
            DependencyProperty.RegisterAttached(
                "ElementString",                // The name of the attached property
                typeof(string),                 // The type of the property (string)
                typeof(ObjectDescription),      // The type that owns the attached property
                new PropertyMetadata(default(string)));   // Metadata, including a default value

        // Setter method for the attached property.
        public static void SetElementString(UIElement element, string value)
        {
            element.SetValue(ElementStringProperty, value);
        }

        // Getter method for the attached property.
        public static string GetElementString(UIElement element)
        {
            return (string)element.GetValue(ElementStringProperty);
        }
    }
}
