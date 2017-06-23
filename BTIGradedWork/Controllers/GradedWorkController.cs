using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace BTIGradedWork.Controllers
{
    public class GradedWorkController : Controller
    {
        private Manager m = new Manager();
        //
        // GET: /GradedWork/
        public ActionResult Index(int id)
        {
            // Get all graded works
            return View(m.GetAllGradedWorks(id));
        }

        //
        // GET: /GradedWork/Details/5
        public ActionResult Details(int id)
        {
            // Fetch graded work with media items
            var gradedwork = m.GetGradedWorkById(id);
            // Check if graded work exists
            if (gradedwork == null)
                return RedirectToAction("Index");
            else
                return View(gradedwork);
        }

        //
        // GET: /GradedWork/Create
        [Authorize(Roles = "Administrator, Teacher")]
        public ActionResult Create(int id)
        {
            // Add default information to the form
            var addForm = new GradedWorkAddForm();
            addForm.DateAssign = DateTime.Now;
            addForm.DateDue = DateTime.Now.AddDays(7);
            addForm.DueDateLate = DateTime.Now.AddDays(7);
            //addForm.Courses = new SelectList(m.GetAllCoursesAsList(), "Id", "Code");
            ViewBag.id = id;
            return View(addForm);
        }

        //
        // POST: /GradedWork/Create
        [Authorize(Roles = "Administrator, Teacher")]
        [HttpPost]
        public ActionResult Create(GradedWorkAdd toBeAdded)
        {
            if(ModelState.IsValid)
            {
                // Add Graded work to data store
                var added = m.AddGradedWork(toBeAdded);
                // Check if graded work exists
                if (added == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Show details
                    return RedirectToAction("Details", new { id = added.Id });
                }
                
            }
            else
            {
                // Error in data, show form again
                var addForm = new GradedWorkAddForm();
                addForm.DateAssign = DateTime.Now;
                addForm.DateDue = DateTime.Now.AddDays(7);
                addForm.DueDateLate = DateTime.Now.AddDays(7);
                //addForm.Courses = new SelectList(m.GetAllCoursesAsList(), "Id", "Code");
                ViewData["id"] = toBeAdded.CourseId;
                return View(addForm);
            }
        }

        //
        // GET: /GradedWork/Edit/5
        [Authorize(Roles = "Administrator, Teacher")]
        public ActionResult Edit(int id)
        {
            // Fetch graded work
            var fetched = m.GetGradedWorkById(id);

            // Checks if work exists
            if (fetched == null)
                return RedirectToAction("Index");
            else
            {
                // Displays edit form
                var gradedWorkEditForm = new GradedWorkEditForm();
                //gradedWorkEditForm.Courses = new SelectList(m.GetAllCoursesAsList(), "Id", "Code");
                ViewBag.id = fetched.CourseId;
                if (String.IsNullOrEmpty(gradedWorkEditForm.InfoURL) || gradedWorkEditForm.InfoURL == "No link available")
                    gradedWorkEditForm.InfoURL = "";
                gradedWorkEditForm = Mapper.Map<GradedWorkEditForm>(fetched);
                return View(gradedWorkEditForm);
            }
        }

        //
        // POST: /GradedWork/Edit/5
        [Authorize(Roles = "Administrator, Teacher")]
        [HttpPost]
        public ActionResult Edit(int id, GradedWorkEdit toBeEdited)
        {
            if(ModelState.IsValid && id == toBeEdited.Id)
            {
                // Edit graded work
                GradedWorkBase edited = m.EditGradedWork(toBeEdited);

                // Check if graded work is valid
                if(edited == null){
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Details", new { id = edited.Id});
                }
            }
            else
            {
                // Error in data, redisplay form
                var gradedWorkEditForm = new GradedWorkEditForm();
                //gradedWorkEditForm.Courses = new SelectList(m.GetAllCoursesAsList(), "Id", "Code");
                ViewBag.id = toBeEdited.CourseId;
                if (String.IsNullOrEmpty(gradedWorkEditForm.InfoURL) || gradedWorkEditForm.InfoURL == "No link available")
                    gradedWorkEditForm.InfoURL = "";
                gradedWorkEditForm = Mapper.Map<GradedWorkEditForm>(toBeEdited);

                return View(gradedWorkEditForm);
            }
        }

        //
        // GET: /GradedWork/Delete/5
        [Authorize(Roles = "Administrator, Teacher")]
        public ActionResult Delete(int? id)
        {
            // Find graded work to be deleted
            var toBeDeleted = m.GetGradedWorkById(id.GetValueOrDefault());

            // Check if item exists
            if (toBeDeleted == null)
                return RedirectToAction("Index");
            else
                return View(toBeDeleted);
        }

        //
        // POST: /GradedWork/Delete/5
        [Authorize(Roles = "Administrator, Teacher")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Attempt to delete gradedWork
            if (m.DeleteGradedWork(id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Details", new { id = id });
            }
        }
    }
}
