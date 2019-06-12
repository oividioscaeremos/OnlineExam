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
       

        // GET: Student/Student
        [Authorize(Roles = "student")]
        public ActionResult Index() 
        {

            
            IList<Exam> result;
            var loggedUser = Database.Session.Query<Users>().FirstOrDefault(c => c.SchoolNumber == HttpContext.User.Identity.Name);
            result = Database.Session.QueryOver<Exam>().Right.JoinQueryOver<Users>(x => x.ExamStudents)
                .Where(c => c.id == loggedUser.id ).List();
            return View(result);
        }

      
        public ActionResult ShowQuestions(int exam_id)
        {
            
            var result = Database.Session.QueryOver<Questions>().Right.JoinQueryOver<Exam>(x => x.ExamQuestions)
                .Where(c => c.id== exam_id).List();

            var exam = Database.Session.Load<Exam>(exam_id);

            return View(new ViewModels.Exam
            {
                questions = result,
                examduration = Int32.Parse(exam.ExamDuration)
                


            });

        }

        //public ActionResult ShowQuestions(List<Questions> )
        //{




        //    return View();

        //}


        
    }
}