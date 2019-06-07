using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineSinav.Areas.Teacher.ViewModels
{
    public class FormData
    {
        public string question_string { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public string correct_answer { get; set; }

        public FormData(string _question_string, string _A, string _B, string _C, string _D, string _E, string _correct_answer)
        {
            question_string = _question_string;
            A = _A;
            B = _B;
            C = _C;
            D = _D;
            E = _E;
            correct_answer = _correct_answer;
        }
    }

    public class GetFormData
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public GetFormData(string name, string val)
        {
            Name = name;
            Value = val;
        }
    }

    public class Questions
    {
        [Display(Name = "Oluşturulacak Sınav Adı")]
        [Required(ErrorMessage = "Sınav adı belirtilmelidir.")]
        [DataType(DataType.Text)]
        public string exam_name { get; set; }

        [Display(Name = "Sınava Girişlerin Başlayacağı Tarih")]
        [Required(ErrorMessage = "Sınavın başlayacağı tarih seçilmelidir.")]
        public DateTime dateFrom { get; set; }

        [Display(Name = "Sınava Girişlerin Sonlanacağı Tarih")]
        [Required(ErrorMessage = "Sınava girişlerin sonlanacağı tarih seçilmelidir.")]
        public DateTime dateTo { get; set; }

        [Display(Name = "Sınav Süresi")]
        [Required(ErrorMessage = "Sınav süresi belirtilmelidir.")]
        public string examDuration { get; set; }

        [Display(Name = "SORU ALANI")]
        public string question_string { get; set; }
        public string A { get; set; }
        public string B { get; set; }
        public string C { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        [Display(Name = "DOĞRU CEVAP")]
        public string correct_answer { get; set; }
    }

    public class examQuests
    {
        public List<Questions> questions { get; set; }
    }
}