using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
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
    /// Interaction logic for School_Class_List_usercontrol.xaml
    /// </summary>
    public partial class School_Student_List_usercontrol : UserControl
    {
        private static string folderpath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FT_SGS", "Classes");
        public int ClassIndexinLoadedList = 0;
        public string ClassFilePath { get; set; }
        public event EventHandler<UserControl> RemoveRequested;
        public event EventHandler<UserControl> ShowProperties;
        public page_manageclasses ParentPage { get; set; }
        public School_Student_List_usercontrol()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Deleted student_info from the class *_domain.json file cannot be recovered once deleted. Do you still want to delete it?", "SGS Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                deleteClass();
            }
            else if (result == MessageBoxResult.No)
            {
                
            }
        }

        private void deleteClass()
        {
            
            if (ParentPage != null)
            {

                RemoveRequested?.Invoke(this, this); // Pass the UserControl instance
                // Delete the PNG file
                File.Delete(ClassFilePath);
            }            
        }

        private void studentbuttonselector_Click(object sender, RoutedEventArgs e)
        {
            ShowProperties?.Invoke(this, this);
        }
    }
}
