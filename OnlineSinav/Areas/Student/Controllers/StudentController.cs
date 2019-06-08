using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSinav.Areas.Teacher.ViewModels;
using OnlineSinav.Models;
using Questions = OnlineSinav.Models.Questions;

namespace OnlineSinav.Areas.Student.Controllers
{
    public class StudentController : Controller
    {
        private object questionalias;

        // GET: Student/Student
        public ActionResult Index()
        {
            var loggedUser = Database.Session.Query<Users>().Where(c => c.SchoolNumber == User.Identity.Name).FirstOrDefault();
            var result = Database.Session.QueryOver<Exam>().Right.JoinQueryOver<Users>(x => x.ExamStudents)
                .Where(c => c.id == loggedUser.id ).List();
            return View(result);
        }

      
        public ActionResult ShowQuestions(int id)
        {
            
            var result = Database.Session.QueryOver<Questions>().Right.JoinQueryOver<Exam>(x => x.ExamQuestions)
                .Where(c => c.id== id).List();

            return View(result);

        }


        
    }
}