using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchoolGradingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event Action<string> PageChanged;

        public MainWindow()
        {
            InitializeComponent();
            NavigateToUtilitySidebar();
            NavigateTomainPage();
            Application.Current.Properties["LightTheme"] = "dark";
        }

        bool isMaximized = false;
        private double savedLeft;
        private double savedTop;
        private double savedWidth;
        private double savedHeight;

        private void NavigateToUtilitySidebar()
        {
            var utilitySidebarPage = new SideBar();
            SideBar.Navigate(utilitySidebarPage);
        }

        private void NavigateTomainPage()
        {
            Page.Navigate(new page_mainmenu());
            Page.Navigated += MainFrame_Navigated;
        }

        public void NavigateToPage(Page page)
        {
            Page.Navigate(page);
            UpdateTitle(page.Title);
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is Page page)
            {
                PageChanged?.Invoke(page.Title);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PageChanged += UpdateTitle;
        }

        public void UpdateTitle(string title)
        {
            TabName.Text = title;
        }

        private void ResizeButtonClick(object sender, RoutedEventArgs e)
        {
            if (isMaximized)
            {
                // Restore the window to its original position and size
                Left = savedLeft;
                Top = savedTop;
                Width = savedWidth;
                Height = savedHeight;
                isMaximized = false;
            }
            else
            {
                // Save the current window position and size
                savedLeft = Left;
                savedTop = Top;
                savedWidth = Width;
                savedHeight = Height;

                // Maximize the window while excluding the taskbar
                var workingArea = SystemParameters.WorkArea;
                Left = workingArea.Left;
                Top = workingArea.Top;
                Width = workingArea.Width;
                Height = workingArea.Height;
                isMaximized = true;
            }
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {   
        WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create a DoubleAnimation to animate the width property
            DoubleAnimation widthAnimation = new DoubleAnimation();
            widthAnimation.From = 32; // Initial width
            widthAnimation.To = 150;   // Final width
            widthAnimation.Duration = new Duration(TimeSpan.FromSeconds(1)); // Duration of the animation

            // Apply the animation to the Width property of the Frame control
            SideBar.BeginAnimation(Frame.WidthProperty, widthAnimation);
        }

        private void OpenLocalFolderr(object sender, RoutedEventArgs e)
        {
            try
            {
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string directoryPath = System.IO.Path.Combine(documentsPath, "FT_SGS");

                // Check if the directory exists
                if (Directory.Exists(directoryPath))
                {
                    // Open the directory in Windows Explorer
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = directoryPath,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
                else
                {
                    MessageBox.Show("The data folder does not exist.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening data folder:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void MiscellaneousButtonClick()
        {
            if (Miscellaneous.Width == 0)
            {
                DoubleAnimation widthAnimation = new DoubleAnimation();
                widthAnimation.To = 160; // Set the desired final width
                widthAnimation.Duration = TimeSpan.FromSeconds(0.25); // Set the duration of the animation
                Miscellaneous.BeginAnimation(Grid.WidthProperty, widthAnimation);
                Timeline.SetDesiredFrameRate(widthAnimation, 120);
            }            
            else
            {
                DoubleAnimation widthAnimation = new DoubleAnimation();
                widthAnimation.To = 0; // Set the desired final width
                widthAnimation.Duration = TimeSpan.FromSeconds(0.25); // Set the duration of the animation
                Miscellaneous.BeginAnimation(Grid.WidthProperty, widthAnimation);
                Timeline.SetDesiredFrameRate(widthAnimation, 60);
            }
        }        

    }
    
}
