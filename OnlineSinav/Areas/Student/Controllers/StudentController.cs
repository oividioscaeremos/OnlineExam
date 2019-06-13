using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OnlineSinav.Areas.Teacher.ViewModels;
using OnlineSinav.Models;
using Questions = OnlineSinav.Models.Questions;

namespace OnlineSinav.Areas.Student.Controllers
{
    [Authorize(Roles = "student")]
    public class StudentController : Controller
    {
        // GET: Student/Student
        public ActionResult Index()
        {
            List<Exam> result = new List<Exam>();
            var loggedUser = Database.Session.Query<Users>().FirstOrDefault(c => c.SchoolNumber == HttpContext.User.Identity.Name);

            result = Database.Session.QueryOver<Exam>().Right.JoinQueryOver<Users>(x => x.ExamStudents)
                .Where(c => c.id == loggedUser.id).List().ToList();

            if (result[0] == null)
            {
                result[0] = new Exam {
                    id = -1
                };
            }
            return View(new OnlineSinav.Areas.Student.ViewModels.StudentIndexShow
            {
                studentExams = result
            });
        }


        public ActionResult ShowQuestions(int exam_id)
        {
            var result = Database.Session.QueryOver<Questions>().Right.JoinQueryOver<Exam>(x => x.ExamQuestions)
                .Where(c => c.id == exam_id).List();

            var exam = Database.Session.Load<Exam>(exam_id);

            return View(new ViewModels.Exam
            {
                questions = result,
                examduration = Int32.Parse(exam.ExamDuration)
            });
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("Home");
        }

    }
}