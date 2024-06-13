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
                    allclasssection_options.Add($"🔗{schoolClass.sClass} {schoolClass.ClassSection}");
                }
            }
            classSectionComboBox.ItemsSource = allclasssection_options;
        }
    }
}
