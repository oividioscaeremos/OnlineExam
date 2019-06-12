using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineSinav.Areas.Student.ViewModels
{
    public class StudentIndexShow
    {
        //public int examID { get; set; }
        //public string examName { get; set; }
        //public DateTime sinav_tarih { get; set; }
        //public DateTime sinav_baslama_saati { get; set; }
        //public DateTime sinav_suresi { get; set; }
        //public int sinavNotu { get; set; }

        public List<OnlineSinav.Models.Exam> studentExams { get; set; }

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