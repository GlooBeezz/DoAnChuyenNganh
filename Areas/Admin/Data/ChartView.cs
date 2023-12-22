using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web2.Areas.Admin.Data
{
    public class ChartView
    {
        public ChartView() { }
        public int NumberOfStudents { get; set; }
        public int NumberOfTeachers { get; set; }
        public int NumberOfCourses { get; set; }
        public int NumberOfClasses { get; set; }
    }
}