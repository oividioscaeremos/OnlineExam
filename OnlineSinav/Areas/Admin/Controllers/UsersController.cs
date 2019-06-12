using OnlineSinav.Models;
using NHibernate.Linq;
using OnlineSinav.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace OnlineSinav.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Admin/Users
        public ActionResult Index()
        {
            return View(new ViewTeacher
            {
                allTeachers = Database.Session.Query<Users>().Where(u => u.Role.RoleName == "teacher").ToList()
            });
        }
        public ActionResult ViewStudents()
        {


            return View(new ViewStudents
            {
                allStudents = Database.Session.Query<Users>().Where(u => u.Role.RoleName == "student").ToList()


            });
        }
        public ActionResult ViewAdmin()
        {


            return View(new ViewAdmin
            {
                allAdmins = Database.Session.Query<Users>().Where(u => u.Role.RoleName == "admin").ToList()

            });
        }

        public ActionResult CreateUser()
        {
            IEnumerable<Roles> currentRoles = Database.Session.Query<Roles>();
            IEnumerable<Department> currentDepts = Database.Session.Query<Department>();
            CreateUser newUser = new CreateUser();

            newUser.allRoles = new SelectList(currentRoles, "id", "RoleName");

            return View(new CreateUser
            {
                allRoles = new SelectList(currentRoles, "id", "RoleName"),
                allDepts = new SelectList(currentDepts, "id", "DeptName")
            });
        }

        private void setUserDept(int deptID, IList<Models.Department> userDept)
        {
            foreach (var departments in Database.Session.Query<Models.Department>())
            {
                if (departments.id == deptID)
                {
                    userDept.Add(departments);
                }
            }
        }

        [HttpPost]
        public ActionResult CreateUser(CreateUser form)
        {
            if (Database.Session.Query<Users>().Any(u => u.SchoolNumber == form.school_number))
            {
                ModelState.AddModelError("school_number", "Belirtilen numaraya ait bir kullanıcı zaten var.");
            }

            if (!ModelState.IsValid)
                return View(form);

            Users newUser = new Users {
                SchoolNumber = form.school_number,
                Role = new Roles
                {
                    id = form.userRoleID,
                    RoleName = (Database.Session.Load<Roles>(form.userRoleID).ToString())
                },
                Name = form.firstname_lastname
            };
            
            setUserDept(form.userDeptID, newUser.depts);            

            newUser.SetPassword(form.password);

            Database.Session.Save(newUser);
            Database.Session.Flush();

            var userRole = Database.Session.Load<Roles>(form.userRoleID).RoleName;
            if (userRole == "admin")
            {
                return RedirectToAction("ViewAdmin");
            }
            else if (userRole == "teacher")
            {
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("ViewStudents");
        }

        public ActionResult DeleteUser(int id)
        {
            var user = Database.Session.Load<Users>(id);

            Database.Session.Delete(user);
            Database.Session.Flush();


            //return RedirectToAction("ViewStudents");
            return RedirectToAction("Index");
            //return RedirectToAction("ViewStudents");

        }

        private void SyncRoles(IList<RolDropDown> checkBoxes, IList<Roles> roles)
        {
            var selectedRoles = new List<Roles>();

            foreach (var role in Database.Session.Query<Roles>())
            {
                var checkbox = checkBoxes.Single(c => c.id == role.id);
                checkbox.name = role.RoleName;

                selectedRoles.Add(role);


            }

            foreach (var toAdd in selectedRoles.Where(t => !roles.Contains(t)))
            {
                roles.Add(toAdd);
            }

            foreach (var toRemove in roles.Where(t => !selectedRoles.Contains(t)).ToList())
            {
                roles.Remove(toRemove);
            }
        }

        public ActionResult EditUser(int id)
        {
            var user = Database.Session.Load<Users>(id);
            IEnumerable<Roles> AllRoles = Database.Session.Query<Roles>();
            IEnumerable<Department> AllDepartment = Database.Session.Query<Department>();


            return View(new CreateUser
            {
                firstname_lastname = user.Name,
                school_number = user.SchoolNumber,
                allRoles = new SelectList(AllRoles),
                SelectedRoleID = System.Convert.ToString(user.Role.id),
                allDepts = new SelectList(AllDepartment),

            });

        }
        [HttpPost]
        public ActionResult EditUser(int id, CreateUser formdata)
        {
            var role = Database.Session.Load<Roles>(System.Convert.ToInt32(formdata.SelectedRoleID));
            var newUser = Database.Session.Load<Users>(id);
            if (Database.Session.Query<Users>().Any(u => u.SchoolNumber == formdata.school_number))
            {
                ModelState.AddModelError("school_number", "Belirtilen numaraya ait bir kullanıcı zaten var.");
            }

            if (!ModelState.IsValid)
                return View(formdata);

            newUser.SchoolNumber = formdata.school_number;
            setUserDept(formdata.userDeptID, newUser.depts);
            newUser.Name = formdata.firstname_lastname;
            newUser.SetPassword(formdata.password);
            newUser.Role = role;

            Database.Session.Update(newUser);
            Database.Session.Flush();
            var user = Database.Session.Load<Users>(id);
            var userRole = user.Role.RoleName;
            
            if (userRole == "admin")
            {
                return RedirectToAction("ViewAdmin");


            }
            else if (userRole == "teacher")
            {
                return RedirectToAction("Index");


            }
            else
                return RedirectToAction("ViewStudents");
        }
        public ActionResult Delete(int id)
        {
            var user = Database.Session.Load<Users>(id);
            var userRole = user.Role.RoleName;
            if (user == null)
            {
                return HttpNotFound();
            }

            Database.Session.Delete(user);
            Database.Session.Flush();
            if (userRole == "admin")
            {
                return RedirectToAction("ViewAdmin");


            }
            else if (userRole == "teacher")
            {
                return RedirectToAction("Index");


            }
            else
                return RedirectToAction("ViewStudents");
        }
    }
}







