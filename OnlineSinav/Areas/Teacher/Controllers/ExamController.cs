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

        public ActionResult ReturnPartial()
        {
            //return PartialView("_QuestionLayout", OnlineSinav.Areas.Teacher.ViewModels.NewExam);
        }

        public ActionResult CreateExam() // Öğretmenin ID'sini alacak
        {
            return View();
        }
    }
}