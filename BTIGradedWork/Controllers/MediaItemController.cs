using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace BTIGradedWork.Controllers
{
    public class MediaItemController : Controller
    {
        private Manager m = new Manager();
        //
        // GET: /MediaItem/
        public ActionResult Index(int id)
        {
            // Get media items
            return View(m.GetAllMediaItems(id));
        }

        public ActionResult Uploads(int? id)
        {
            // Extract number from passed-in argument
            int lookup = id.GetValueOrDefault();

            // Attempt to fetch the vehicle
            var mi = m.GetUpload(lookup);

            // Check if media item exists
            if (mi == null)
            {
                return HttpNotFound();
            }
            else
            {
                // Return photo bytes and set the Content-type header
                return File(mi.Content, mi.MediaType);
            }
        }

        //
        // GET: /MediaItem/Details/5
        public ActionResult Details(int id)
        {
            // Get a media item by id
            var mediaitem = m.GetMediaItemById(id);
            // Checks if media item exists
            if (mediaitem == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Shows media item 
                return View(mediaitem);
            }
        }

        //
        // GET: /MediaItem/Create
        [Authorize(Roles = "Administrator, Teacher")]
        public ActionResult Create(int id)
        {
            ViewBag.id = id;
            return View();
        }

        //
        // POST: /MediaItem/Create
        [Authorize(Roles = "Administrator, Teacher")]
        [HttpPost]
        public ActionResult Create(MediaItemAdd toBeAdded)
        {
            if(ModelState.IsValid)
            {
                // Add media item
                var added = m.AddMediaItem(toBeAdded);
                // Check if media item exists
                if (added == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Shows details 
                    return RedirectToAction("Details", "GradedWork", new { id = added.GradedWorkId});
                }
            }
            else
            {
                // Error in data, show form again
                ViewData["id"] = toBeAdded.GradedWorkId;
                return View();
            }
        }

        //
        // GET: /MediaItem/Edit/5
        [Authorize(Roles = "Administrator, Teacher")]
        public ActionResult Edit(int id)
        {
            // Fetch media item
            var fetched = m.GetMediaItemById(id);

            // Check if item exists
            if (fetched == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Display edit form
                var mediaEditForm = new MediaItemEditForm();
                ViewBag.id = fetched.GradedWorkId;
                mediaEditForm = Mapper.Map<MediaItemEditForm>(fetched);
                return View(mediaEditForm);
            }
        }

        //
        // POST: /MediaItem/Edit/5
        [Authorize(Roles = "Administrator, Teacher")]
        [HttpPost]
        public ActionResult Edit(int id, MediaItemEdit toBeEdited)
        {
            if(ModelState.IsValid && id == toBeEdited.Id)
            {
                // Edit media item
                MediaItemBase edited = m.EditMediaItem(toBeEdited);

                // Check if item is valid
                if(edited == null){
                    return RedirectToAction("Index");
                }else{
                return RedirectToAction("Details", new{id = edited.Id});
                }
            }
            else
            {
                // Error in data, redisplay edit form
                var mediaEditForm = new MediaItemEditForm();
                ViewBag.id = toBeEdited.GradedWorkId;
                mediaEditForm = Mapper.Map<MediaItemEditForm>(toBeEdited);
                return View(mediaEditForm);
            }
        }

        //
        // GET: /MediaItem/Delete/5
        [Authorize(Roles = "Administrator, Teacher")]
        public ActionResult Delete(int? id)
        {
            // Find item to be deleted
            var toBeDeleted = m.GetMediaItemById(id.GetValueOrDefault());

            // Check if item exists
            if (toBeDeleted == null)
                return RedirectToAction("Index");
            else
                return View(toBeDeleted);
        }

        //
        // POST: /MediaItem/Delete/5
        [Authorize(Roles = "Administrator, Teacher")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // Attempt to delete media item
            if(m.DeleteMediaItem(id))
            {
                return RedirectToAction("Index", "GradedWork");
            }
            else
            {
                return RedirectToAction("Details", new { id = id });
            }
        }
    }
}
