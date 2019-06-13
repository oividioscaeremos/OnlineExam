using OnlineSinav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSinav.Areas.Student.ViewModels
{
    public class StudentIndexShow
    {
        public List<Models.Exam> studentExams { get; set; }

        public List<ExamResult> examResult { get; set; }

        public int counter {
            get {
                return counter;
            }
            set {
                counter = value;
            }
        }
    }
}