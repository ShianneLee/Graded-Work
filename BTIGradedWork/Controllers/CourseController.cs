using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace BTIGradedWork.Controllers
{
    public class CourseController : Controller
    {
        private Manager m = new Manager();
        //
        // GET: /Course/
        public ActionResult Index()
        {
            // Get all courses
            return View(m.GetAllCoursesWithTeacher());
        }

        //
        // GET: /Course/Details/5
        public ActionResult Details(int id)
        {
            // Gets course by ID
            var course = m.GetCourseById(id);

            // Check if course exists
            if (course == null)
                return RedirectToAction("Index");
            else
                return View(course);
        }

        //
        // GET: /Course/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            var addForm = new CourseAddForm();
            addForm.Teachers = new SelectList(m.GetAllTeachersAsList(), "Id", "Name");
            return View(addForm);
        }

        //
        // POST: /Course/Create
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create(CourseAdd toBeAdded)
        {
            if(ModelState.IsValid)
            {
                // Add Course to data store
                var added = m.AddCourse(toBeAdded);

                // Check if course exists
                if (added == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Details", new { id = added.Id});
                }
                
            }
            else
            {
                // Error in data, redisplay form
                var addForm = Mapper.Map<CourseAddForm>(toBeAdded);
                addForm.Teachers = new SelectList(m.GetAllTeachersAsList(), "Id", "Name", toBeAdded.TeacherId);
                return View(addForm);
            }
        }

        //
        // GET: /Course/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            // Get course object
            CourseBase fetched = m.GetCourseById(id);

            // Check if course exists
            if (fetched == null)
                return RedirectToAction("Index");
            else
            {
                // Display editable form
                var courseEditForm = new CourseEditForm();
                ViewBag.TeacherId = new SelectList(m.GetAllTeachersAsList(), "Id", "Name", fetched.TeacherId);
                if (String.IsNullOrEmpty(courseEditForm.OutlineUrl) || courseEditForm.OutlineUrl == "No link available")
                    courseEditForm.OutlineUrl = "";
                courseEditForm = Mapper.Map<CourseEditForm>(fetched);
                return View(courseEditForm);
            }
        }

        //
        // POST: /Course/Edit/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(int id, CourseEdit course)
        {
            if (ModelState.IsValid && id == course.Id)
            {
                // Edit course
                CourseBase edited = m.EditCourse(course);

                // Check if course is valid
                if (edited == null)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("Details", new { id = edited.Id });
            }
            else
            {
                // Error in data, redisplay edit form
                var courseEditForm = new CourseEditForm();
                courseEditForm = Mapper.Map<CourseEditForm>(course);
                courseEditForm.Teachers = new SelectList(m.GetAllTeachersAsList(), "Id", "Name", course.TeacherId);
                if (String.IsNullOrEmpty(courseEditForm.OutlineUrl) || courseEditForm.OutlineUrl == "No link available")
                    courseEditForm.OutlineUrl = "";
                courseEditForm = Mapper.Map<CourseEditForm>(course);
                return View(courseEditForm);
            }
        }

        //
        // GET: /Course/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            // Fetch object to be deleted
            CourseBase toBeDeleted = m.GetCourseById(id.GetValueOrDefault());

            // Check if object exists
            if (toBeDeleted == null)
                return RedirectToAction("Index");
            else
                return View(toBeDeleted);
        }

        //
        // POST: /Course/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Remove object from data store
            if (m.DeleteCourse(id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Details", new { id = id });
            }
        }

        ////
        //// GET: /Course/CourseSelection/5
        //public ActionResult CourseSelection(int id)
        //{
        //    ViewBag.id = id;
        //    ViewBag.Courses = new SelectList(m.GetCourseSelectionList(), "Id", "Code");
        //    return View();
        //}

        //// POST: /Course/CourseSelection/5
        //[HttpPost]
        //public ActionResult CourseSelection(CourseSelect toBeAdded)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var added = m.AddCourseSelection(toBeAdded);
        //        if (added == null)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            return RedirectToAction("Details", "Student", new{})
        //        }
        //    }
        //}
        
    }
}
