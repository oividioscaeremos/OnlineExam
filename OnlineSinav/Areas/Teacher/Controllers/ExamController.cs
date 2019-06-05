using Newtonsoft.Json;
using OnlineSinav.Areas.Teacher.ViewModels;
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
        public ActionResult CreateExam(string formdata) // BURASI ŞU ANDA ÇALIŞMIYOR ELLEYENİ TERS ÇEVİRİR KÖTÜ HAREKETLERE MARUZ BIRAKIRIM.
        { 

            var abc = formdata;
            formdata.Replace("'\'","");

            var abcde = formdata;
            examQuests eq = JsonConvert.DeserializeObject<examQuests>(formdata);

            Questions[] examQuests = (Questions[])JsonConvert.DeserializeObject(formdata);
            
            var abcd = formdata[1];
            return RedirectToAction("index");

        }
    }
}