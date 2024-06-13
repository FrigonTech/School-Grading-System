using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchoolGradingSystem
{
    /// <summary>
    /// Interaction logic for page_registerstudent.xaml
    /// </summary>
    public partial class page_registerstudent : Page
    {
        public int classindex;
        public int studentindex;
        public List<SchoolClass> allLoadedClasses = new List<SchoolClass>();
        private static string folderpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FT_SGS", "Classes");
        public page_registerstudent()
        {
            InitializeComponent();
            LoadAllClassesFromFolder(folderpath);
            DataContext = this;
        }

        public string currentDate
        {
            get
            {
                return DateTime.Now.ToString("dd/MM/yyyy");
            }
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
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., display an error message)
                Console.WriteLine($"Error reading JSON files: {ex.Message}");
            }
        }

        private void classSectionComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> allclasssection_options = new List<string> {"--Select a Class--"};
            foreach (SchoolClass schoolClass in allLoadedClasses)
            {
                {
                    allclasssection_options.Add($"🔗 {schoolClass.sClass} {schoolClass.ClassSection}");
                }
            }
            classSectionComboBox.ItemsSource = allclasssection_options;
        }

        private void ChangePicButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;)|*.png;",
                InitialDirectory = @"C:\" // Set the initial directory (optional)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected file path
                string selectedImagePath = openFileDialog.FileName;

                // Determine the destination directory and file path
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string directoryPath = System.IO.Path.Combine(documentsPath, "FT_SGS", "StudentImages");
                Directory.CreateDirectory(directoryPath); // Ensure the directory exists

                // Determine the destination file path
                string fileName = System.IO.Path.GetFileName(selectedImagePath);
                string destinationFilePath = System.IO.Path.Combine(directoryPath, fileName);

                try
                {
                    // Copy the image file to the destination
                    File.Copy(selectedImagePath, destinationFilePath, true);

                    // delete previous picture by filename
                    CleanupOldPngFiles();

                    // Load the new image into the Image control
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(destinationFilePath, UriKind.RelativeOrAbsolute);
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Ensure the file is fully loaded before use
                    bitmapImage.EndInit();
                    this.stuimage.Source = bitmapImage;                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing image:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CleanupOldPngFiles()
        {
            try
            {
                string oldpicpath = stuimage.Source.ToString();
                if ( oldpicpath == "" || oldpicpath == "pack://application:,,,/SchoolGradingSystem;component/Assets/Utility/DarkModeButtons/dummy_dark.png"
                    || oldpicpath == "pack://application:,,,/SchoolGradingSystem;component/Assets/Utility/LightModeButtons/dummy_light.png")
                {
                    
                }
                else
                {
                    try
                    {
                        File.Delete(oldpicpath.Substring(8, oldpicpath.Length - 8));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.Message}","config", MessageBoxButton.OK);
                    }
                    //set default pic back in case of any disruptions
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(Application.Current.Properties["Theme"]?.ToString() == "Light"? "pack://application:,,,/SchoolGradingSystem;component/Assets/Utility/DarkModeButtons/dummy_dark.png"
                        : "pack://application:,,,/SchoolGradingSystem;component/Assets/Utility/LightModeButtons/dummy_light.png", UriKind.RelativeOrAbsolute);
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Ensure the file is fully loaded before use
                    bitmapImage.EndInit();
                    this.stuimage.Source = bitmapImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting file(s): {ex.Message}", "⚠️ Path Error", MessageBoxButton.OK);
            }
        }

        private void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            //Check all conditions for saving a student's profile!!
            if (stuname.Text == "" || stuname.Text == null || classSectionComboBox.SelectedItem == null || dob.SelectedDate == null || joiningdate.SelectedDate == null)
            {
                MessageBox.Show("Student name, class, section, joining date and date of birth are necessary for registrering a student in a class.", "⚠️ Required Field Null", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                string[] SelectedClassAndSection = classSectionComboBox.SelectedItem.ToString().Split(' ');
                int.TryParse(stuadmissionno.Text, out int stuadmissionnumber);
                int.TryParse(sturollno.Text, out int sturollnumber);
                School_StudentInfo newStudent = new School_StudentInfo()
                {
                    studentName = stuname.Text,
                    studentPic = stuimage.Source.ToString(),//Substring(8, stuimage.Source.ToString().Length - 8
                    studentAddress = stuaddress.Text,
                    studentClass = $"{SelectedClassAndSection[1].Trim()} {SelectedClassAndSection[2].Trim()}",
                    studentAdmissionno = stuadmissionnumber,
                    studentRollno = sturollnumber,
                    studentDOB = dob.SelectedDate,
                    studentJoiningfrom = joiningdate.SelectedDate,
                };

                // Check if the class and section already exist in the list
                bool classExists = allLoadedClasses.Any(existingClass => existingClass.sClass.ToString() == SelectedClassAndSection[1].Trim() && existingClass.ClassSection == SelectedClassAndSection[2].Trim());
                if (classExists)
                {
                    // Find the index of the class and section in the list
                    int index = allLoadedClasses.FindIndex(existingClass =>
                        existingClass.sClass.ToString() == SelectedClassAndSection[1].Trim() &&
                        existingClass.ClassSection == SelectedClassAndSection[2].Trim());

                    // Output or further processing
                    if (index != -1)
                    {
                        int.TryParse(SelectedClassAndSection[1].Trim(), out int Classs);
                        if (allLoadedClasses[index].Students == null)
                        {
                            allLoadedClasses[index].Students = new List<School_StudentInfo> {newStudent};
                        }
                        else
                        {
                            allLoadedClasses[index].Students.Add(newStudent);
                        }                        
                        SaveNewClass(allLoadedClasses[index], Classs, SelectedClassAndSection[2].Trim());
                    }
                    else
                    {
                        Console.WriteLine("Class and section not found.");
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
    }
   
}
