using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
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
                result[0] = new Exam
                {
                    id = -1
                };
            }

            List<ExamResult> examResults = new List<ExamResult>();
            examResults = Database.Session.Query<ExamResult>().Where(e => e.student.id == loggedUser.id).ToList();
            if (examResults.Count == 0)
            {
                examResults.Add(new ExamResult
                {
                    id = -1,
                    exam = new Exam
                    {
                        id = -1,
                    },
                });
            }
            List<Exam> examAvailableNow = new List<Exam>(); // Süresi başlamış olan sınavlar, bu listeye çekilecek

            if (result[0].id != -1) // Sonuçlar listesinin boş olup olmadığına bakılmakta.
            {
                foreach (var exam in result)
                {
                    var _exam = Database.Session.Load<Exam>(exam.id);

                    var exDate = _exam.examStart.Date;
                    var nowDate = DateTime.Now.Date;

                    var exTimeOfDay = _exam.examStart.TimeOfDay;
                    var nowTimeOfDay = DateTime.Now.TimeOfDay;
                    if (_exam.examStart.Date == DateTime.Now.Date && (_exam.examStart.AddMinutes(15) >= DateTime.Now) && (_exam.examStart <= DateTime.Now))
                    {
                        examAvailableNow.Add(exam); // Sınav başlamışsa ve öğrenci sınav başladıktan 15 dakika 
                    }                               // sonraya kadar sınav ekranına geldiyse bu arkadaşa sınav bilgilerini atacağız.
                }
            }

            if(examAvailableNow.Count == 0)
            {
                examAvailableNow.Add(new Exam {
                    id = -1,
                }); // Maksat liste boş dönmesin, view'da sıkıntı çıkıyor sonra.
            }

            return View(new OnlineSinav.Areas.Student.ViewModels.StudentIndexShow
            {
                studentExams = examAvailableNow,
                examResult = examResults
            });
        }

        public ActionResult ShowQuestions(int exam_id)
        {
            Users currentUser = new Users();
            currentUser = Database.Session.Query<Users>().FirstOrDefault(s => s.SchoolNumber == HttpContext.User.Identity.Name);
            
            var result = Database.Session.QueryOver<Questions>().Right.JoinQueryOver<Exam>(x => x.ExamQuestions)
                .Where(c => c.id == exam_id).List();

            Exam exam = new Exam();
            exam = Database.Session.Load<Exam>(exam_id);

            return View(new ViewModels.Exam
            {
                questions = result,
                examduration = Int32.Parse(exam.ExamDuration),
                studentExamID = exam_id,
                studentID = currentUser.id
            });
        }

        public struct question_answer
        {
            public string question_number;
            public char answer;
        }

        public void SendExamResult(int examid,int studentid, string answers)
        {
            Exam thisExam = new Exam();
            Users thisStudent = new Users();
            thisExam = Database.Session.Load<Exam>(examid);
            thisStudent = Database.Session.Load<Users>(studentid);
            string _students_answers = "";
            string _correct_answers = "";

            var examDetailsArray = JsonConvert.DeserializeObject<List<GetFormData>>(answers);

            int totalResult = 0,
                point_per_question = (100 / thisExam.ExamQuestions.Count);

            question_answer[] questionsAndAnswers = new question_answer[examDetailsArray.Count];

            for (int i = 0; i < examDetailsArray.Count; i++)
            {
                questionsAndAnswers[i].answer = examDetailsArray[i].Name.ToString()[examDetailsArray[i].Name.Length - 1];
                questionsAndAnswers[i].question_number = examDetailsArray[i].Name.ToString().TrimEnd('A','B','C','D','E');
            }

            foreach(var answer in questionsAndAnswers)
            {
                var getQuestionFromDatabase = Database.Session.Load<Questions>(Convert.ToInt32(answer.question_number));
                _students_answers += answer.answer +",";
                _correct_answers += getQuestionFromDatabase.TrueAnswer + ",";
                if(getQuestionFromDatabase.TrueAnswer.ToString().ToUpper() == answer.answer.ToString().ToUpper())
                {
                    totalResult += point_per_question;
                }
            }

            if (totalResult / point_per_question == thisExam.ExamQuestions.Count)
                totalResult = 100;

            ExamResult exam_result = new ExamResult {
                exam = thisExam,
                student = thisStudent,
                students_answers = _students_answers.TrimEnd(','),
                correct_answers = _correct_answers.TrimEnd(','),
                teacher = thisExam.teacher,
                result = totalResult
            };

            Database.Session.Save(exam_result);
            Database.Session.Flush();
         }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("Home");
        }
    }
}