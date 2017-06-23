using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace BTIGradedWork.Controllers
{
    public class TeacherController : Controller
    {
        private Manager m = new Manager();
        //
        // GET: /Teacher/
        public ActionResult Index()
        {
            // Fetch all teachers
            return View(m.GetAllTeachers());
        }

        [Authorize(Roles = "Administrator, Teacher")]
        public ActionResult TeacherCourses()
        {
            return View(m.GetTeacherCourseList());
        }

        //
        // GET: /Teacher/Details/5
        public ActionResult Details(int id)
        {
            // Get object by identifier
            var fetched = m.GetTeacherById(id);

            // Checks if object exists
            if (fetched == null)
                return RedirectToAction("Index");
            else
                // Display object found
                return View(fetched);
        }

        //#### NOT BEING USED
        //
        // GET: /Teacher/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Teacher/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        //#### END OF NOT BEING USED

        //
        // GET: /Teacher/Edit/5

        [Authorize(Roles = "Administrator, Teacher")]
        public ActionResult Edit(int id)
        {
            // Get teacher by id
            TeacherBase fetched = m.GetTeacherById(id);

            // Check if teacher exists
            if (fetched == null)
                return RedirectToAction("Index");
            else
            {
                // Display edit form if teacher exists
                var teacherEditForm = new TeacherEditForm();
                teacherEditForm = Mapper.Map<TeacherEditForm>(fetched);
                return View(teacherEditForm);
            }
        }

        //
        // POST: /Teacher/Edit/5
        [Authorize(Roles = "Administrator, Teacher")]
        [HttpPost]
        public ActionResult Edit(int id, TeacherEdit teacher)
        {
            if (ModelState.IsValid & id == teacher.Id)
            {
                // Edit teacher
                TeacherBase editedItem = m.EditTeacher(teacher);

                // Check if teacher exists
                if (editedItem == null)
                    return RedirectToAction("Index");
                else
                {
                    // Displays newly updated teacher
                    return RedirectToAction("Details", new { id = editedItem.Id });
                }
            }
            else
            {
                // Error in data, redisplays form
                var teacherEditForm = new TeacherEditForm();

                teacherEditForm = Mapper.Map<TeacherEditForm>(teacher);

                return View(teacherEditForm);
            }
        }

        //
        // GET: /Teacher/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            // Fetch the teacher by id
            TeacherBase toBeDeleted = m.GetTeacherById(id.GetValueOrDefault());

            // Checks if teacher exists
            if (toBeDeleted == null)
                return RedirectToAction("Index");
            else
                // Displays teacher to be deleted
                return View(toBeDeleted);
        }

        //
        // POST: /Teacher/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Attempt to delete teacher
            if (m.DeleteTeacher(id))
                // Successfully deleted
                return RedirectToAction("Index");
            else
                return RedirectToAction("Details", new { id = id });
        }
    }
}
