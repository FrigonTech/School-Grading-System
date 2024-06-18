using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using System.Windows.Threading;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Bson;
using System.Windows.Media.Animation;

namespace SchoolGradingSystem
{
    /// <summary>
    /// Interaction logic for page_schoolregistry.xaml
    /// </summary>    
    public partial class page_mainmenu : Page, INotifyPropertyChanged
    {
        private string _currentDateTime;
        public event PropertyChangedEventHandler PropertyChanged;
        public static string schoolNAME;
        public static string schoolCONTACT;
        public static string schoolLogoo;
        public static string schoolAFFILIATION;
        public static string sadresss;
        public static string schoolPRINCI;
        public static string schoolregisterdate;
        public static string schoolAFFNUM;
        bool Isalreadyscheduling;
        DateTime selectedDate;

        private bool SchoolInfoFileExists()
        {
            // Get the path to the "Documents" folder
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Define the file path
            string filePath = System.IO.Path.Combine(documentsPath, "FT_SGS", "myschoolinfo", "myschoolinfo.json");

            // Check if the file exists
            return File.Exists(filePath);
        }

        public page_mainmenu()
        {
            InitializeComponent();
            LoadSchoolInfoinApplication();
            DataContext = this;
            StartTimer();
            UpdateDateTime();
        }

        public string CurrentDateTime
        {
            get => _currentDateTime;
            private set
            {
                if (_currentDateTime != value)
                {
                    _currentDateTime = value;
                    OnPropertyChanged(nameof(CurrentDateTime));
                }
            }
        }

        private void StartTimer()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (s, e) => UpdateDateTime();
            timer.Start();
        }

        private void UpdateDateTime()
        {
            CurrentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void addSchedule(object sender, RoutedEventArgs e)
        {
            if (!Isalreadyscheduling) 
            {
                Isalreadyscheduling = true;
                //prompt a calender//
                // // Animate the calender//
                // // // Add Background Blur Anim
                // Create the height animation//
                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    To = 25,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)) // Set the duration of the animation
                };
                this.scheduledatepicker.BeginAnimation(HeightProperty, heightAnimation);
                this.AddScheduleButton.Content = "Esc";
            }
            else
            {
                Isalreadyscheduling = false;
                //prompt a calender//
                // // Animate the calender//
                // // // Add Background Blur Anim
                // Create the height animation//
                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.1)) // Set the duration of the animation
                };
                this.scheduledatepicker.BeginAnimation(HeightProperty, heightAnimation);
                this.AddScheduleButton.Content = "+";
            }
        }

        private void SchedulesCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ensure sender is Calendar
            if (sender is DatePicker calendar)
            {
                // Check if a date is selected
                if (calendar.SelectedDate.HasValue)
                {
                    DateTimeOffset currentDate = DateTimeOffset.Now;
                    selectedDate = calendar.SelectedDate.Value;
                    double differenceInDays = Math.Ceiling((selectedDate - currentDate).TotalDays);
                    if (differenceInDays <= 0) 
                    {
                        EndSchedulingProcess(selectedDate, false);
                    }
                    else
                    {
                        EndSchedulingProcess(selectedDate, true);
                    }
                }
            }
        }

        private void EndSchedulingProcess(DateTimeOffset date, bool sucess)
        {
            if (sucess)
            {
                //add a new schedule U.C.
                var MySchedule = new SchoolGradingSystem.scheduled_date_element
                {
                    S_Date = date,
                    Width = 289
                };
                MySchedule.Init_Day_Text();
                this.schedulespanel.Children.Add(MySchedule);
                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.1)) // Set the duration of the animation
                };
                this.scheduledatepicker.BeginAnimation(HeightProperty, heightAnimation);
                Isalreadyscheduling = false;
                this.AddScheduleButton.Content = "+";
            }
            else
            {
                MessageBox.Show("Request Unavailable", "Date Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    To = 0,
                    Duration = new Duration(TimeSpan.FromSeconds(0.1)) // Set the duration of the animation
                };
                this.scheduledatepicker.BeginAnimation(HeightProperty, heightAnimation);
                Isalreadyscheduling = false;
                this.AddScheduleButton.Content = "+";
            }            
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SchoolInfo schoolInfoStruct;

        private void LoadSchoolInfoinApplication()
        {
            LoadSchoolInfoFromFile();         
        }

        private void LoadSchoolInfoFromFile()
        {
            // Get the path to the "Documents" folder
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Define the file path
            string filePath = System.IO.Path.Combine(documentsPath, "FT_SGS", "myschoolinfo", "myschool_info.json");

            // Check if the file exists
            if (File.Exists(filePath))
            {

                try
                {
                    // Read the JSON string from the file
                    string jsonString = File.ReadAllText(filePath);

                    // Deserialize the JSON string to a SchoolInfo object
                    SchoolInfo schoolInfoStruct = JsonConvert.DeserializeObject<SchoolInfo>(jsonString);
                    schoolNAME = schoolInfoStruct.schoolName;
                    schoolCONTACT = schoolInfoStruct.schoolContact;
                    schoolAFFILIATION = schoolInfoStruct.schoolAffiliation;
                    schoolPRINCI = schoolInfoStruct.princi;
                    sadresss = schoolInfoStruct.saddress;
                    schoolAFFNUM = schoolInfoStruct.affiliationnum;
                    schoolregisterdate = schoolInfoStruct.lastregdate;
                    int pathLength = schoolInfoStruct.schoolLogo.Length;
                    schoolLogoo = schoolInfoStruct.schoolLogo;//Substring(22, pathLength-22);
                    InitAllInfo();
                }
                catch (Exception ex)
                {
                    // Handle any exceptions, such as invalid JSON format
                    Console.WriteLine($"Error loading school info: {ex.Message}");
                }
            }         
        }

        private void InitAllInfo()
        {
            this.SchlName.Text = schoolNAME;
            this.Affiliation.Text = schoolAFFILIATION;
            this.SchoolContacT.Text = schoolCONTACT;
            if (schoolLogoo == null | schoolLogoo == string.Empty | Directory.Exists(System.IO.Path.GetDirectoryName(schoolLogoo)))
            {
                schoolLogoo = "pack://application:,,,/SchoolGradingSystem;component/Assets/Images/DemoSchoolLogo.png";
            }

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(schoolLogoo, UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = bitmapImage;

            // Assuming `Logo` is a Rectangle or another element that supports Fill property
            this.Logo.Fill = imageBrush;
        }

        public string GetLogo()
        {
            return schoolLogoo;
        }

        private void ManageClasses_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.UpdateTitle("Manage Classes");
            }
            NavigationService.Navigate(new page_manageclasses());
        }

        private void StudentRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.UpdateTitle("Student Registration");
            }
            NavigationService.Navigate(new page_registerstudent());
        }

        private void FacultyRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.UpdateTitle("Faculty Registration");
            }
            NavigationService.Navigate(new page_registerfaculty());
        }

        private void SchoolRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.UpdateTitle("School Registration");
            }
            NavigationService.Navigate(new page_registerschool());
        }

        public string description { get; set; } = "Hover Over Any Element";

        private void OnMouseHover(object sender, MouseEventArgs e)
        {
            if (sender is UIElement element)
            {
                string description = ObjectDescription.GetElementString(element);
                //Add logic to update alttextbox beneath mouse
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is UIElement element)
            {
                string description = ObjectDescription.GetElementString(element);
                //Add logic to update alttextbox beneath mouse
            }
        }

        private void ReloadScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            // Get all child elements (schedules) in the StackPanel
            List<scheduled_date_element> schedules = new List<scheduled_date_element>();
            foreach (var child in schedulespanel.Children)
            {
                if (child is scheduled_date_element schedule)
                {
                    schedules.Add(schedule);
                }
            }

            // Sort schedules by differenceInDays in ascending order
            schedules.Sort((s1, s2) =>
            {
                // Calculate differenceInDays for each schedule
                double differenceInDays1 = (s1.S_Date.Value - DateTimeOffset.Now).TotalDays;
                double differenceInDays2 = (s2.S_Date.Value - DateTimeOffset.Now).TotalDays;

                // Compare based on differenceInDays (ascending order)
                return differenceInDays1.CompareTo(differenceInDays2);
            });

            // Clear current children in StackPanel
            schedulespanel.Children.Clear();

            // Re-add schedules in sorted order
            foreach (var schedule in schedules)
            {
                schedulespanel.Children.Add(schedule);
            }
        }
    }
    
}
