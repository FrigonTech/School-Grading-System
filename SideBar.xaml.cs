using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Management;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Timers;
using System.Threading;


namespace SchoolGradingSystem
{
    /// <summary>
    /// Interaction logic for SideBar.xaml
    /// </summary>
    public partial class SideBar : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static bool CanAnimateWidth = true;
        

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.NavigateToPage(new page_mainmenu());
            }
        }
        
        public SideBar()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void LightThemeChange(object sender, RoutedEventArgs e)
        {
            string newThemePath = "/Themes/DarkTheme.xaml";
            var newtheme = (ResourceDictionary)Application.LoadComponent(resourceLocator: new Uri(newThemePath, UriKind.Relative)); ;

            if (Application.Current.Properties["LightTheme"].ToString() == "light")
            {
                Application.Current.Properties["LightTheme"] = "dark";
                newThemePath = "/Themes/DarkTheme.xaml";
                newtheme = (ResourceDictionary)Application.LoadComponent(resourceLocator: new Uri(newThemePath, UriKind.Relative));                
            }
            else
            {
                Application.Current.Properties["LightTheme"] = "light";
                newThemePath = "/Themes/LightTheme.xaml";
                newtheme = (ResourceDictionary)Application.LoadComponent(resourceLocator: new Uri(newThemePath, UriKind.Relative));                
            }

            Application.Current.Resources.MergedDictionaries.RemoveAt(index: 0);
            Application.Current.Resources.MergedDictionaries.Insert(0, newtheme);

            
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                CanAnimateWidth = !CanAnimateWidth;
                mainWindow.MiscellaneousButtonClick();
                // Get the parent window of the page
                Window window = Window.GetWindow(this);

                // Access the Frame control inside the window
                Frame frame = window.FindName("SideBar") as Frame;

                // Create and start the animation
                if (frame != null & !CanAnimateWidth)
                {
                    DoubleAnimation widthAnimation = new DoubleAnimation();
                    widthAnimation.To = 38; // Set the desired final width
                    widthAnimation.Duration = TimeSpan.FromSeconds(0.25); // Set the duration of the animation
                    frame.BeginAnimation(Frame.WidthProperty, widthAnimation);
                    Timeline.SetDesiredFrameRate(widthAnimation, 144);
                }
            }
        }

        private void WrapPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            // Get the parent window of the page
            Window window = Window.GetWindow(this);

            // Access the Frame control inside the window
            Frame frame = window.FindName("SideBar") as Frame;

            // Create and start the animation
            if (frame != null)
            {
                DoubleAnimation widthAnimation = new DoubleAnimation();
                widthAnimation.To = 38; // Set the desired final width
                widthAnimation.Duration = TimeSpan.FromSeconds(0.25); // Set the duration of the animation
                frame.BeginAnimation(Frame.WidthProperty, widthAnimation);
                Timeline.SetDesiredFrameRate(widthAnimation, 144);
            }
        }

        private void WrapPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                
                // Get the parent window of the page
                Window window = Window.GetWindow(this);

                // Access the Frame control inside the window
                Frame frame = window.FindName("SideBar") as Frame;

                // Create and start the animation
                if (frame != null & CanAnimateWidth)
                {
                    DoubleAnimation widthAnimation = new DoubleAnimation();
                    widthAnimation.To = 160; // Set the desired final width
                    widthAnimation.Duration = TimeSpan.FromSeconds(0.25); // Set the duration of the animation
                    frame.BeginAnimation(Frame.WidthProperty, widthAnimation);
                    Timeline.SetDesiredFrameRate(widthAnimation, 144);
                }
            }
        }
    }
}
