using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchoolGradingSystem
{
    /// <summary>
    /// Interaction logic for page_manageclasses.xaml
    /// </summary>
    public partial class page_manageclasses : Page
    {
        private static string folderpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FT_SGS", "Classes");
        public List<SchoolClass> allLoadedClasses = new List<SchoolClass>();
        int currentclassindex = -1;


        public page_manageclasses()
        {
            InitializeComponent();
            DataContext = this;
            LoadAllClassesFromFolder(folderpath);
        }

        // Load and parse all JSON files from the specified folder
        public void LoadAllClassesFromFolder(string folderPath)
        {
            try
            {
                // Get all JSON files in the folder
                string[] jsonFiles = Directory.GetFiles(folderPath, "*_domain_info.json");
                int index = 0;

                foreach (string filePath in jsonFiles)
                {
                    index++;
                    string jsonString = File.ReadAllText(filePath);
                    SchoolClass loadedClass = JsonConvert.DeserializeObject<SchoolClass>(jsonString);

                    // Check if the class already exists in the list
                    if (!allLoadedClasses.Any(existingClass => existingClass.sClass == loadedClass.sClass && existingClass.ClassSection == loadedClass.ClassSection))
                    {
                        allLoadedClasses.Add(loadedClass);
                        LoadClassUserCOntrol(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., display an error message)
                Console.WriteLine($"Error reading JSON files: {ex.Message}");
            }
        }

        private void LoadClassUserCOntrol(string filepath)
        {
            foreach (SchoolClass loadedClass in allLoadedClasses)
            {
                // Create an instance of your user control
                var myUserControl = new School_Class_List_usercontrol();
                //edit its properties
                myUserControl.ClassIndexinLoadedList = allLoadedClasses.IndexOf(loadedClass);
                myUserControl.ParentPage = this;
                myUserControl.ClassFilePath = filepath;
                myUserControl.Class.Text = loadedClass.sClass.ToString();
                myUserControl.ClassSectionn.Text = loadedClass.ClassSection.ToString();
                myUserControl.OccupiedSeatss.Text = loadedClass.OccupiedSeats.ToString();
                myUserControl.Total_Seatss.Text = loadedClass.TotalSeats.ToString();
                //myUserControl.Width = 800;
                //myUserControl.Height = 41;
                myUserControl.RemoveRequested += UserControl_RemoveRequested;
                myUserControl.ShowProperties += UserControl_ShowPorperties;
                myUserControl.ShowStudents += UserControl_ShowStudents;
                // Add it to the StackPanel
                this.Classpanel.Children.Add(myUserControl);
            }
        }

        private void LoadStudentsUserControl(int ClassIndex)
        {
            if(currentclassindex == ClassIndex)
            {
                            
            }
            else
            {
                currentclassindex = ClassIndex;
                this.StudentsPanel.Children.Clear();
                foreach (School_StudentInfo student in allLoadedClasses[ClassIndex].Students)
                {
                    //Create an instance of your user control
                    var mystudentscontrol = new School_Student_List_usercontrol();
                    mystudentscontrol.StudentNAMEE.Text = student.studentName;
                    this.StudentsPanel.Children.Add(mystudentscontrol);
                    mystudentscontrol.ShowProperties += StudentControl_ShowPorperties;
                }
            }            
        }

        private void StudentControl_ShowPorperties(object sender, UserControl e)
        {
            if(sender is School_Student_List_usercontrol studentcontrol)
            {
                foreach(School_StudentInfo student in allLoadedClasses[studentcontrol.ClassIndexinLoadedList].Students)
                {
                    string Property = ($"Name: {student.studentName}\n\nRoll No.:{student.studentRollno}\n\nAdmission No.:{student.studentAdmissionno}\n\nAddress:{student.studentAddress}\n\n" +
                        $"Date of Birth: {student.studentDOB}\n\nJoining Date: {student.studentJoiningfrom}");
                    this.properties.Text = Property;
                }
            }
        }

        private void UserControl_RemoveRequested(object sender, UserControl e)
        {
            // Remove the specific UserControl instance from the parent container
            if (sender is School_Class_List_usercontrol userControl)
            {
                // Get the ClassIndex from the userControl
                int classIndex = userControl.ClassIndexinLoadedList;
                allLoadedClasses.RemoveAt( classIndex );
                this.Classpanel.Children.Remove(userControl);                
            }
        }

        private void UserControl_ShowStudents(object sender, UserControl e)
        {
            if (sender is School_Class_List_usercontrol usercontrol)
            {
                //Manage Focusable
                this.Classpanel.Focusable = false;
                this.StudentsPanel.Focusable = true;
                this.studentsborder.Focusable = true;
                this.studentsback_back.Focusable= true;

                //Manage HitTestable
                this.Classpanel.IsHitTestVisible = false;
                this.StudentsPanel.IsHitTestVisible = true;
                this.studentsborder.IsHitTestVisible = true;
                this.studentsback_back.IsHitTestVisible = true;

                //Manage Visibility
                this.Classpanel.Visibility = Visibility.Hidden;
                this.StudentsPanel.Visibility = Visibility.Visible;
                this.studentsborder.Visibility = Visibility.Visible;
                this.studentsback_back.Visibility = Visibility.Visible;

                LoadStudentsUserControl(usercontrol.ClassIndexinLoadedList);
            }
        }

        private void UserControl_ShowPorperties(object sender, UserControl e)
        {
            // Remove the specific UserControl instance from the parent container
            if (sender is School_Class_List_usercontrol userControl)
            {
                //Get Properties from class
                
                string Property = ($"Class: {userControl.Class.Text}{userControl.ClassSectionn.Text} \nStudents: {userControl.OccupiedSeatss.Text}/{userControl.Total_Seatss.Text}\n\n---Students:---\n");
                foreach(School_StudentInfo studentInfo in allLoadedClasses[userControl.ClassIndexinLoadedList].Students)
                {
                    Property += ($"{studentInfo.studentName}\n");
                }
                this.properties.Text = Property;
            }
        }

        private void SearchBar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.SearchBar.Text == "Search...")
            {
                this.SearchBar.Text = string.Empty;
            }
        }

        private void SearchBar_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.SearchBar.Text))
            {
                this.SearchBar.Text = "Search...";
            }
        }

        private void add_classes_up_button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(this.Class_Standard.Text, out int currentClass))
            {
                if (currentClass >= 1 && currentClass < 12)
                {
                    currentClass++; // Increment the class number
                    this.Class_Standard.Text = currentClass.ToString();
                }
                else
                {
                    // Handle the case when the class number is already 12
                    // (e.g., display an error message)
                }
            }
        }

        private void add_classes_down_button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(this.Class_Standard.Text, out int currentClass))
            {
                if (currentClass > 1 && currentClass < 12)
                {
                    currentClass--; // Increment the class number
                    this.Class_Standard.Text = currentClass.ToString();
                }
                else
                {
                    // Handle the case when the class number is already 12
                    // (e.g., display an error message)
                }
            }
        }

        private int currentsectionnumber = 0; // Initialize at 0 (for "A")

        private void add_sections_up_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentsectionnumber >= 0 && currentsectionnumber < classSuffix.Length - 1)
            {
                currentsectionnumber++; // Increment the section index
                this.sections.Text = classSuffix[currentsectionnumber].ToString();
            }
            else
            {
                // Handle the case when the section index is out of bounds
                // (e.g., display an error message or wrap around to the first section)
            }
        }

        private void add_sections_down_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentsectionnumber > 0 && currentsectionnumber < classSuffix.Length)
            {
                currentsectionnumber--; // Decrement the section index
                this.sections.Text = classSuffix[currentsectionnumber].ToString();
            }
            else
            {
                // Handle the case when the section index is out of bounds
                // (e.g., display an error message or wrap around to the last section)
            }
        }

        private string[] classSuffix = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        private void add_classes_Click(object sender, RoutedEventArgs e)
        {
            if (this.AddClassBox.Visibility == Visibility.Visible)
            {
                this.AddClassBox.Visibility = Visibility.Hidden;
                this.AddClassBox.Focusable = false;
            }
            else
            {
                this.AddClassBox.Visibility = Visibility.Visible;
                this.AddClassBox.Focusable = true;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true; // Suppress the input
            }
        }

        private void add_classes_ok_button_Click(object sender, RoutedEventArgs e)
        {            
            if (int.TryParse(this.Class_Standard.Text, out int currentClass) && int.TryParse(this.AddClassTotalSeats.Text, out int totalseats))
            {
                // Check if the class and section already exist in the list
                bool classExists = allLoadedClasses.Any(existingClass => existingClass.sClass == currentClass && existingClass.ClassSection == this.sections.Text);

                if (totalseats <= 0)
                {
                    MessageBox.Show("A class cannot have 0 alloted seats! Please consider rechecking your inputs.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (!classExists)
                    {
                        SchoolClass NewClass = new SchoolClass()
                        {
                            sClass = currentClass,
                            TotalSeats = totalseats,
                            ClassSection = this.sections.Text
                        };
                        SaveNewClass(NewClass, currentClass, this.sections.Text);
                    }
                    else
                    {
                        // Show a message or handle the case where the class already exists
                        MessageBoxResult result = MessageBox.Show("This class and section already exists. Still proceeding will overrite the previous info. Do you wanna continue?", "Duplicate Class", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            SchoolClass NewClass = new SchoolClass()
                            {
                                sClass = currentClass,
                                TotalSeats = totalseats,
                                ClassSection = this.sections.Text
                            };
                            SaveNewClass(NewClass, currentClass, this.sections.Text);
                        }
                    }
                }                
            }
        }

        private void SaveNewClass(SchoolClass newclassinfo, int currentclass, string section)
        {

            // Serialize the struct to JSON
            string jsonString = JsonConvert.SerializeObject(newclassinfo);

            try
            {
                // Define the file path
                string filePath = System.IO.Path.Combine(folderpath, $"{currentclass}_{section}_domain_info.json");

                if (!File.Exists(folderpath)) 
                {
                    Directory.CreateDirectory(folderpath);
                }

                // Save the JSON string to the file
                File.WriteAllText(filePath, jsonString);
                LoadAllClassesFromFolder(folderpath);
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving 'Class' info: {ex.Message}", "Configuration", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void reload_data_Click(object sender, RoutedEventArgs e)
        {
            LoadAllClassesFromFolder(folderpath);
        }

        private void studentsback_back_Click(object sender, RoutedEventArgs e)
        {
            //Manage Focusable
            this.Classpanel.Focusable = true;
            this.StudentsPanel.Focusable = false;
            this.studentsborder.Focusable = false;
            this.studentsback_back.Focusable = false;

            //Manage HitTestable
            this.Classpanel.IsHitTestVisible = true;
            this.StudentsPanel.IsHitTestVisible = false;
            this.studentsborder.IsHitTestVisible = false;
            this.studentsback_back.IsHitTestVisible = false;

            //Manage Visibility
            this.Classpanel.Visibility = Visibility.Visible;
            this.StudentsPanel.Visibility = Visibility.Hidden;
            this.studentsborder.Visibility = Visibility.Hidden;
            this.studentsback_back.Visibility = Visibility.Hidden;
        }
    }
}
