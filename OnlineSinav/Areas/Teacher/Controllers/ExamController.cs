using Newtonsoft.Json;
using OnlineSinav.Areas.Teacher.ViewModels;
using OnlineSinav.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineSinav.Areas.Teacher.Controllers
{
    [Authorize(Roles = "teacher")]
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


        public ActionResult EditExam(int examID)
        {
            var exam = new Exam();

            exam = Database.Session.Load<Exam>(examID);


            var dateSplitted = exam.examStart.ToString("dd/MM/yyyy HH:mm:ss");
            //var hour = Convert.ToInt32(dateSplitted[1][0] + dateSplitted[1][1]);
            string day,month,hour,minute;

            day = exam.examStart.Day.ToString();
            month = exam.examStart.Month.ToString();
            hour = exam.examStart.Hour.ToString();
            minute = exam.examStart.Minute.ToString();

            if(day.Length == 1)
            {
                day = "0" + day;
            }
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            if (hour.Length == 1)
            {
                hour = "0" + hour;
            }
            if (minute.Length == 1)
            {
                minute = "0" + minute;
            }

            string dateString = exam.examDate.ToString("dd/MM/yyyy") + " " + hour+":"+minute+":00";

            //{6/13/2019 5:30:00 PM}

            var abcde = DateTime.ParseExact(exam.examStart.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", null);

            return View(new EditExamIndex
            {
                selectedExam = exam,
                fullDate = DateTime.ParseExact(exam.examStart.ToString("dd/MM/yyyy HH:mm"),"dd/MM/yyyy HH:mm", null)
            });
        }

        [HttpPost]
        public void EditExam(string formdata, string examDetails, int examID)
        {
            Users currentTeacher = new Users();
            Exam examToEdit = new Exam();
            Exam examBeforeEdit = new Exam();
            examToEdit = Database.Session.Load<Exam>(examID); // düzenleyeceğimiz sınavı çekiyoruz.

            IList<Models.Questions> currentExamsQuestions;

            var questionsArray = JsonConvert.DeserializeObject<List<FormData>>(formdata); // JSON.Stringify'dan model kullanarak normal array'e çeviriyoruz.
            var examDetailsArray = JsonConvert.DeserializeObject<List<GetFormData>>(examDetails); // JSON.Stringify'dan model kullanarak normal array'e çeviriyoruz.         

            var examDuration = examDetailsArray[1].Value; // sınav süresi verisi

            string[] examDateStr = examDetailsArray[2].Value.ToString().Split('-'); // sınav tarih verisini bölüyoruz çünkü sayfada tarih verisi değiştirilmemişse
                                                                                    // hata veriyor, arada - olmadığı için. Burda bölünüp bölünmediğine bakıyoruz kısaca.

            DateTime examFullDate;
            string dateString;

            if (examDateStr.Count<string>() == 1)
            {
                examFullDate = Convert.ToDateTime(examDateStr[0]);
                dateString = examFullDate.ToString("dd/MM/yyyy HH:mm");
                examFullDate = DateTime.ParseExact(dateString,"dd/MM/yyyy HH:mm", null);

            }
            else
            {
                var degisken = examDateStr[0].TrimEnd(' ') + " " + examDateStr[1].TrimStart(' ');

                examFullDate = DateTime.ParseExact(degisken,"dd/MM/yyyy HH:mm",null);
                dateString = examFullDate.ToString("dd/MM/yyyy HH:mm");


            }

            currentExamsQuestions = examToEdit.ExamQuestions; // 

            int currentQuestionCount = currentExamsQuestions.Count;
            for (int i = 0; i < currentQuestionCount; i++)
            {
                var question = currentExamsQuestions[0];
                currentExamsQuestions.Remove(question); // önceki soruların hepsini siliyoruz, yenilerini ekleyip kaydedeceğiz.
            }

            Database.Session.Update(examToEdit);

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
                    dept_id = examToEdit.Department
                };
                examToEdit.ExamQuestions.Add(toAdd);
                Database.Session.Save(toAdd);
            };

            var deneme = DateTime.ParseExact(examFullDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToShortDateString();
            
            examToEdit.ExamName = examDetailsArray[0].Value;
            examToEdit.ExamDuration = examDetailsArray[1].Value;
            examToEdit.examDate = Convert.ToDateTime(deneme);
            examToEdit.examStart = examFullDate;

            examToEdit.examEnd = examFullDate.AddMinutes(Convert.ToInt32(examDuration));
            



            var examsDetails = examToEdit;
            var allQuestions = examToEdit.ExamQuestions;

            Database.Session.Update(examToEdit);
            Database.Session.Flush();
        }

        [HttpPost]
        public void DeleteExam(int examID)
        {
            Exam examToDelete = new Exam();
            Department dept = new Department();

            examToDelete = Database.Session.Load<Exam>(examID);
            dept.id = examToDelete.Department.id;
            int totalQuestions = examToDelete.ExamQuestions.Count;

            for (int i = 0; i < totalQuestions; i++)
            {
                var question = examToDelete.ExamQuestions[0];
                examToDelete.ExamQuestions.Remove(question);
                Database.Session.Delete(question);
            }
            Database.Session.Delete(examToDelete);
            Database.Session.Flush();
        }

        [HttpPost]
        public void CreateExam(string formdata, string examDetails)
        {
            var questionsArray = JsonConvert.DeserializeObject<List<FormData>>(formdata);
            var examDetailsArray = JsonConvert.DeserializeObject<List<GetFormData>>(examDetails);

            Users currentTeacher = new Users();

            currentTeacher = Database.Session.Query<Users>().FirstOrDefault(u => u.SchoolNumber == HttpContext.User.Identity.Name);

            string[] dateTime;
            dateTime = examDetailsArray[2].Value.ToString().Split('-');

            var examDuration = examDetailsArray[1].Value;

            var hour = Convert.ToInt32(examDuration) / 60;
            var minutes = Convert.ToInt32(examDuration) % 60;

            string[] examStart = dateTime[1].TrimStart(' ').Split(':');

            var examDate = DateTime.ParseExact(dateTime[0].TrimEnd(' '), "dd/MM/yyyy", null);
            var examStartt = DateTime.ParseExact((dateTime[0].TrimEnd(' ') + " " + dateTime[1].TrimEnd(' ').TrimStart(' ')), "dd/MM/yyyy HH:mm", null);

            DateTime examEndTime;
            examEndTime = examStartt.AddMinutes(Convert.ToDouble(examDuration));

            Exam newExam = new Exam
            {
                ExamName = examDetailsArray[0].Value,
                ExamDuration = examDetailsArray[1].Value,
                examDate = DateTime.ParseExact(dateTime[0].TrimEnd(' '), "dd/MM/yyyy", null),
                examStart = DateTime.ParseExact((dateTime[0].TrimEnd(' ') + " " + dateTime[1].TrimEnd(' ').TrimStart(' ')), "dd/MM/yyyy HH:mm", null),
                examEnd = examEndTime,
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
                           .List();


            if (_Choosen[0] == null)
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

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("Home");

        }
    }
}