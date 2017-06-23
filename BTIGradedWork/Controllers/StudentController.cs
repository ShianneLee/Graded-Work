using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace BTIGradedWork.Controllers
{
    public class StudentController : Controller
    {
        private Manager m = new Manager();
        //
        // GET: /Student/
        public ActionResult Index()
        {
            // Get all students
            return View(m.GetAllStudents());
        }

        [Authorize(Roles = "Administrator, Student")]
        public ActionResult StudentCourses()
        {
            return View(m.GetStudentCourseList());
        }

        //
        // GET: /Student/Details/5
        [Authorize(Roles = "Administrator, Student")]
        public ActionResult Details(int id)
        {
            // Get object by identifier
            var fetched = m.GetStudentWithCourses(id);

            // Checks if object exists
            if (fetched == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Display object found
                return View(fetched);
            }
        }

        //#### NOT BEING USED
        //
        // GET: /Student/Create
        //[Authorize(Roles = "Administrator")]
        //public ActionResult Create()
        //{
        //    var studentAddForm = new StudentAddForm();
        //    return View(studentAddForm);
        //}

        ////
        //// POST: /Student/Create
        //[Authorize(Roles = "Administrator")]
        //[HttpPost]
        //public ActionResult Create(StudentAdd newItem)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var addedItem = m.AddStudent(newItem);
        //        if (addedItem == null)
        //            return RedirectToAction("Index");
        //        else
        //            return RedirectToAction("Details", new { id = addedItem.Id });
        //    }
        //    else
        //    {
        //        // Errors with incoming data
        //        var studentAddForm = new StudentAddForm();
        //        return View(studentAddForm);
        //    }
        //}

        //#### END OF NOT BEING USED 

        //
        // GET: /Student/Edit/5
        
        [Authorize(Roles = "Administrator, Student")]
        public ActionResult Edit(int id)
        {
            // Get student by id
            StudentBase fetched = m.GetStudentById(id);

            // Check if student exists
            if (fetched == null)
                return RedirectToAction("Index");
            else
            {
                // Display edit form if student exists
                var studentEditForm = new StudentEditForm();
                studentEditForm = Mapper.Map<StudentEditForm>(fetched);
                return View(studentEditForm);
            }
        }

        //
        // POST: /Student/Edit/5
        [Authorize(Roles = "Administrator, Student")]
        [HttpPost]
        public ActionResult Edit(int id, StudentEdit student)
        {
            if (ModelState.IsValid & id == student.Id)
            {
                // Edit student
                StudentBase editedItem = m.EditStudent(student);

                // Checks if update was successful
                if (editedItem == null)
                    return RedirectToAction("Index");
                else
                {
                    // Displays details if successful
                    return RedirectToAction("Details", new { id = editedItem.Id });
                }
            }
            else
            {
                // Error on form, redisplays form
                var studentEditForm = new StudentEditForm();

                studentEditForm = Mapper.Map<StudentEditForm>(student);

                return View(studentEditForm);
            }
        }

        //
        // GET: /Student/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            // Fetch student by id
            StudentBase toBeDeleted = m.GetStudentById(id.GetValueOrDefault());

            // Checks if student exists
            if (toBeDeleted == null)
                return RedirectToAction("Index");
            else
                // Displays student to be deleted
                return View(toBeDeleted);
        }

        //
        // POST: /Student/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            // Attempt to delete student
            if (m.DeleteStudent(id))
            {
                // Successfully deleted
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Details", new { id = id });
            }

        }
    }
}
