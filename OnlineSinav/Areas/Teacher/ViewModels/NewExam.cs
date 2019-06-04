using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineSinav.Areas.Teacher.ViewModels
{
    public class NewExam
    {
        /*
          Create.Table("Exam")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("exam_name").AsString(128)
                .WithColumn("exam_time_from").AsDate()
                .WithColumn("exam_time_to").AsDate()
                .WithColumn("dept_id").AsInt32().ForeignKey("Department", "id").OnDelete(System.Data.Rule.Cascade);
         */
        [Display(Name ="Oluşturulacak Sınav Adı")]
        [Required(ErrorMessage = "Sınav adı belirtilmelidir.")]
        [DataType(DataType.Text)]
        public string exam_name { get; set; }

        [Display(Name = "Sınava Girişlerin Başlayacağı Tarih")]
        [Required(ErrorMessage = "Sınavın başlayacağı tarih seçilmelidir.")]
        public DateTime dateFrom { get; set; }

        [Display(Name = "Sınava Girişlerin Sonlanacağı Tarih")]
        [Required(ErrorMessage = "Sınava girişlerin sonlanacağı tarih seçilmelidir.")]
        public DateTime dateTo { get; set; }
    }

    public class Questions
    {
        public IList<Questions> questions { get; set; }
        public string question_string { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public string correct_answer { get; set; }
    }
}