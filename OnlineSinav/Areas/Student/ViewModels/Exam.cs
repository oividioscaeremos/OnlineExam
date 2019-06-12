using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineSinav.Models;

namespace OnlineSinav.Areas.Student.ViewModels
{
    public class Exam
    {
        public int examduration { get; set; }
        public IList<Questions> questions { get; set; }


    }
}