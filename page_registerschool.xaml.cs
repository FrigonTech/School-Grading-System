using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SchoolGradingSystem.Properties;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;

namespace SchoolGradingSystem
{
    /// <summary>
    /// Interaction logic for page_registerstudent.xaml
    /// </summary>
    public partial class page_registerschool : Page
    {
        private string CurrentLogo;
        
        public page_registerschool()
        {
            InitializeComponent();
            InitRegistryForm();
            DataContext = this;
            
        }

        private void InitRegistryForm()
        {
            CurrentLogo = page_mainmenu.schoolLogoo;
            BitmapImage bitmapImage = new BitmapImage();
            // Check if the file exists
            bool fileExists = File.Exists(CurrentLogo);
            if (CurrentLogo == null | CurrentLogo == string.Empty | fileExists)
            {
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("pack://application:,,,/WpfApp2;SchoolGradingSystem/Assets/Images/DemoSchoolLogo.png", UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();
                this.schllogo.Source = bitmapImage;
            }
            else
            {
                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(CurrentLogo, UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();
                this.schllogo.Source = bitmapImage;
            }           
            
            this.SchlName.Text = page_mainmenu.schoolNAME;
            this.contact.Text = page_mainmenu.schoolCONTACT;
            this.Aff.Text = page_mainmenu.schoolAFFILIATION; 
            this.schlregdate.Text = page_mainmenu.schoolregisterdate;
            this.Principal.Text = page_mainmenu.schoolPRINCI;
            this.schlnumber.Text = page_mainmenu.schoolAFFNUM;
            this.schladdress.Text = page_mainmenu.sadresss;
        }

        public void SaveRegInDisk(object sender, RoutedEventArgs e)
        {
            SchoolInfo schoolInformation = new SchoolInfo
            {
                schoolName = SchlName.Text,
                schoolAffiliation = Aff.Text,
                schoolContact = contact.Text,
                schoolLogo = schllogo.Source.ToString(),
                saddress = schladdress.Text,
                schoolRegDate = schlregdate.Text,
                princi = Principal.Text,
                affiliationnum = schlnumber.Text,
                lastregdate = schlregdate.Text
            };
            SaveSchoolInfo(schoolInformation);
        }

        private void OpenImageFileButton_Click(object sender, RoutedEventArgs e)
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
                string directoryPath = System.IO.Path.Combine(documentsPath, "FT_SGS", "myschoolinfo");
                Directory.CreateDirectory(directoryPath); // Ensure the directory exists

                // Determine the destination file path
                string fileName = System.IO.Path.GetFileName(selectedImagePath);
                string destinationFilePath = System.IO.Path.Combine(directoryPath, fileName);

                try
                {
                    // Copy the image file to the destination
                    File.Copy(selectedImagePath, destinationFilePath, true);

                    // Load the new image into the Image control
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(destinationFilePath, UriKind.RelativeOrAbsolute);
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Ensure the file is fully loaded before use
                    bitmapImage.EndInit();
                    this.schllogo.Source = bitmapImage;

                    // Cleanup old PNG files in the directory
                    CleanupOldPngFiles(directoryPath, destinationFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing image:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CleanupOldPngFiles(string directoryPath, string fileToKeep)
        {
            try
            {
                // Get all PNG files in the directory
                string[] pngFiles = Directory.GetFiles(directoryPath, "*.png");

                // Loop through each PNG file
                foreach (string pngFile in pngFiles)
                {
                    // Check if the file is not the one to keep
                    if (!pngFile.Equals(fileToKeep, StringComparison.OrdinalIgnoreCase))
                    {
                        // Delete the PNG file
                        File.Delete(pngFile);
                        Console.WriteLine($"Deleted: {pngFile}");
                    }
                }

                Console.WriteLine("Deletion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting files: {ex.Message}");
            }
        }


        private void SaveSchoolInfo(SchoolInfo schoolInfo)
        {
            
            // Serialize the struct to JSON
            string jsonString = JsonConvert.SerializeObject(schoolInfo);

            try
            {
                // Get the path to the "Documents" folder
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // Create the directory if it doesn't exist
                string directoryPath = System.IO.Path.Combine(documentsPath, "FT_SGS", "myschoolinfo");
                Directory.CreateDirectory(directoryPath);

                // Define the file path
                string filePath = System.IO.Path.Combine(directoryPath, "myschool_info.json");

                // Save the JSON string to the file
                File.WriteAllText(filePath, jsonString);
                MessageBox.Show("School Registry Saved!", "SGS Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving school info: {ex.Message}", "SGS Info", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
