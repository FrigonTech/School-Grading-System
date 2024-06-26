﻿using System;
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
        public string studentClass;
        public int studentRollno;
        public int studentAdmissionno;
        public string studentAddress;
        public DateTimeOffset? studentJoiningfrom;
        public DateTimeOffset? studentDOB;
        public string studentPic;
    }

    public struct School_FacultyInfo
    {
        public string facultyName;
        public string facultyClass;
        public int facultyAdmissionno;
        public string facultyAddress;
        public DateTimeOffset? facultyJoiningfrom;
        public DateTimeOffset? facultyDOB;
        public string facultyPic;
        public string Subject;
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
                    return ($"{Students.Count}");
                }
                
            }
        }
        public List<School_StudentInfo> Students { get; set; }
        public School_FacultyInfo Teacher { get; set; }
    }

}
