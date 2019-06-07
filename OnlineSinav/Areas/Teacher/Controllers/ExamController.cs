using Newtonsoft.Json;
using OnlineSinav.Areas.Teacher.ViewModels;
using OnlineSinav.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineSinav.Areas.Teacher.Controllers
{
    public class ExamController : Controller
    {
        // GET: Teacher/Exam
        // Mevcut öğretmen için eklediği sınavları çekecek ve görüntüleyecek
        // Sadece sınavın id'sini ve ismini vs çekecek
        // Sınava giren öğrenciler veya girmesi gereken öğrencileri görüntülemek için
        // farklı bir action'a gönderecek.
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateExam() // Öğretmenin ID'sini alacak
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateExam(string formdata, string examDetails){ // BURASI ŞU ANDA ÇALIŞMIYOR ELLEYENİ TERS ÇEVİRİR KÖTÜ HAREKETLERE MARUZ BIRAKIRIM.

            var questionsArray = JsonConvert.DeserializeObject<List<FormData>>(formdata);
            var examDetailsArray = JsonConvert.DeserializeObject<List<GetFormData>>(examDetails);

            var currentTeacher = Database.Session.Query<Users>().FirstOrDefault(u => u.SchoolNumber == HttpContext.User.Identity.Name);
            Exam newExam = new Exam
            {
                ExamName = examDetailsArray[0].Value,
                ExamDuration = examDetailsArray[1].Value,
                dateFrom = Convert.ToDateTime(examDetailsArray[2].Value),
                dateTo = Convert.ToDateTime(examDetailsArray[3].Value),
                Department = currentTeacher.depts.FirstOrDefault()
            };

            foreach (var question in questionsArray)
            {
                Models.Questions toAdd = new Models.Questions {
                    QuestName = question.question_string,
                    TrueAnswer = question.correct_answer,
                    Answer1 = question.A,
                    Answer2 = question.B,
                    Answer3 = question.C,
                    Answer4 = question.D,
                    Answer5 = question.E,
                    dept_id = newExam.Department
                };
                newExam.ExamQuestions.Add(toAdd);
                Database.Session.Save(toAdd);
            };

            Database.Session.Save(newExam);

            Database.Session.Flush();

            return View();
        }
    }
}