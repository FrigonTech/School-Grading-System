using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SchoolGradingSystem
{
    public struct School_StudentInfo
    {
        public string studentName;
        public int studentClass;
        public int studentRollno;
        public int studentAdmissionno;
        public string studentAddress;
        public string studentJoining;
        public string studentDOB;
        public ImageSource studentPic;
    }

    public class SchoolInfo
    {
        public string schoolName { get; set; }
        public string schoolAffiliation { get; set; }
        public string schoolContact { get; set; }
        public string schoolLogo { get; set; }
        public string princi { get; set; }
        public string schoolRegDate { get; set; }
        public string affiliationnum { get; set; }
        public string lastregdate { get; set; }
        public string saddress { get; set; }
    }


    public class SchoolClass
    {
        public int sClass { get; set; }
        public string ClassSection { get; set; }
        public int TotalSeats { get; set; }
        public string OccupiedSeats 
        {
            get
            {
                if (Students == null)
                {
                    return ("0");
                }
                else
                {
                    return ($"{Students.Length}/{TotalSeats}");
                }
                
            }
        }
        public School_StudentInfo[] Students { get; set; }
    }

}
