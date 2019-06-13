using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OnlineSinav.Models;

namespace OnlineSinav.Areas.Student.ViewModels
{
    public class Exam
    {
        public int examduration { get; set; }
        public IList<Questions> questions { get; set; }
        public int studentExamID { get; set; }
        public int studentID { get; set; }
    }

    public class ForEachQuestion
    {
        [Display(Name ="SORU")]
        public string question_string { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public bool correct_answer { get; set; }

        public int counter { get; set; }
        public Questions question { get; set; }
    }

}