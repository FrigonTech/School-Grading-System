using System;
using System.Collections.Generic;
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
    /// Interaction logic for scheduled_date_element.xaml
    /// </summary>
    public partial class scheduled_date_element : UserControl
    {
        public DateTimeOffset? S_Date;
        public event EventHandler<UserControl> ClearScheduleInstance;

        public scheduled_date_element()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Update currentDate at the time of comparison
        private DateTimeOffset currentDate => DateTimeOffset.Now;

        public void Init_Day_Text()
        {
            if (S_Date.HasValue)
            {
                // Calculate the difference in days
                double differenceInDays = Math.Ceiling((S_Date.Value - currentDate).TotalDays);
                if(differenceInDays > 99)
                {
                    this.text_DAYLEFT.Text = "99+";
                }
                this.text_DAYLEFT.Text = differenceInDays.ToString();
                SetTextColorAccToTimeLeft(differenceInDays);
            }
        }

        private void SetTextColorAccToTimeLeft(double differenceInDays)
        {
            if (differenceInDays < 5)
            {
                if (differenceInDays < 3)
                {
                    settextcolor("RedG");
                }
                else
                {
                    settextcolor("YellowG");
                }
            }            
            else
            {
                settextcolor("GreenG");
            }
        }

        private void settextcolor(string color_to_set)
        {
            if (Application.Current.Resources[color_to_set] is LinearGradientBrush brush)
            {
                this.text_TO.Foreground = brush;
                this.text_GO.Foreground = brush;
                this.Text_DAYS.Foreground = brush;
                this.text_DAYLEFT.Foreground = brush;
            }
        }

        private void clearSchedule(object sender, RoutedEventArgs e)
        {
            ClearScheduleInstance?.Invoke(this, this);
        }
    }
}
