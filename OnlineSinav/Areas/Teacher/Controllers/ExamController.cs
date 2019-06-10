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
            Users _teacher = new Users();
            IList<Exam> thisTeachersExams;


            _teacher = Database.Session.Query<Users>().FirstOrDefault(u => u.SchoolNumber == HttpContext.User.Identity.Name);

            thisTeachersExams = Database.Session.Query<Exam>().Where(e => e.teacher.id == _teacher.id).ToList();

            return View(new TeacherIndexViewModel
            {
                exams = thisTeachersExams,
                teacher = _teacher
            });
        }

        public ActionResult CreateExam()
        {
            return View();
        }

        [HttpPost]
        public void CreateExam(string formdata, string examDetails)
        {


            var questionsArray = JsonConvert.DeserializeObject<List<FormData>>(formdata);
            var examDetailsArray = JsonConvert.DeserializeObject<List<GetFormData>>(examDetails);

            var currentTeacher = Database.Session.Query<Users>().FirstOrDefault(u => u.SchoolNumber == HttpContext.User.Identity.Name);
            Exam newExam = new Exam
            {
                ExamName = examDetailsArray[0].Value,
                ExamDuration = examDetailsArray[1].Value,
                dateFrom = Convert.ToDateTime(examDetailsArray[2].Value),
                dateTo = Convert.ToDateTime(examDetailsArray[3].Value),
                Department = currentTeacher.depts.FirstOrDefault(),
                teacher = currentTeacher
            };

            foreach (var question in questionsArray)
            {
                Models.Questions toAdd = new Models.Questions
                {
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


            var abcde = newExam;

            Database.Session.Save(newExam);
            Database.Session.Flush();


        }

        public ActionResult AssignExam(int teacherID, int examID) // burası tüm sınavları çektiğim ekran
        {
            var teacher = Database.Session.Load<Users>(teacherID);
            var thisTeachersExams = Database.Session.Query<Exam>().Where(e => e.teacher.id == teacher.id).ToList();

            var dept = teacher.depts.FirstOrDefault();

            var studentsForThisDept = Database.Session.Query<Users>() // Bu tüm öğrencilerin listesi
                .Where(s => s.depts.FirstOrDefault() == dept).Where(s => s.Role.RoleName == "student")
                .OrderBy(s => s.SchoolNumber).ToList();



            var _Choosen = Database.Session.QueryOver<Users>() // sınava öğrenci ata dediğimde seçilmiş ve seçilmemiş olarak iki farklı şekilde liste oluşturuyor,
                           .Right.JoinQueryOver<Exam>(x => x.exams) // bu seçilmiş öğrencilerin listesi
                           .Where(c => c.id == examID)
                           .Where(c => c.Department == dept)
                           .List();


            if(_Choosen[0] == null)
            {
                _Choosen = new List<Users>();
                

                return View(new AssignExamIndex
                {
                    selectedExamID = examID,
                    studentsNotChoosen = studentsForThisDept,
                    studentsChoosed = _Choosen.OrderBy(c => c.SchoolNumber).ToList()
                });
            }
            else
            {
                for (int i = 0; i < _Choosen.Count; i++) // eğer seçilmiş öğrenci varsa tüm öğrencilerin listesinden kaldırıp o şekilde gönderiyorum view'a
                {
                    if (studentsForThisDept.Contains(_Choosen[i]))
                    {
                        studentsForThisDept.Remove(_Choosen[i]);
                    }
                }

                return View(new AssignExamIndex
                {
                    selectedExamID = examID,
                    studentsNotChoosen = studentsForThisDept,
                    studentsChoosed = _Choosen.OrderBy(c => c.SchoolNumber).ToList()
                });
            }
        }

        [HttpPost]
        public void AssignStudent(int studentID, int examId)
        {
            var student = Database.Session.Load<Users>(studentID);

            var exam = Database.Session.Load<Exam>(examId);

            exam.ExamStudents.Add(student);

            Database.Session.Update(exam);
            Database.Session.Flush();
        }

        [HttpPost]
        public void ReAssignStudent(int studentID, int examId)
        {
            var student = Database.Session.Load<Users>(studentID);

            var exam = Database.Session.Load<Exam>(examId);

            exam.ExamStudents.Remove(student);

            Database.Session.Update(exam);
            Database.Session.Flush();
        }
    }
}