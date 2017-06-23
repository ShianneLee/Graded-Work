using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BTIGradedWork.Controllers
{
    // Used to create 'add media item' form
    public class MediaItemAddForm
    {

        [Display(Name = "Media Item Name")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        //[Display(Name = "Graded Work")]
        //public int GradeWorkId { get; set; }
    }

    // Used to check gathered data from user
    public class MediaItemAdd
    {
        [Required]
        [Display(Name="Media Item Name")]
        [StringLength(50, ErrorMessage = "{0} can contain up to {1} characters.")]
        [RegularExpression(@"^[A-Za-z0-9\-\'()&\s]*$", ErrorMessage = "{0} must be alphabetic characters.")]
        public string Name { get; set; }
        [Required]
        [Display(Name="Media Item Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int GradedWorkId { get; set; }
        public HttpPostedFileBase ItemUpload { get; set; }
    }

    // Used to as the edit form for the Media Item
    public class MediaItemEditForm : MediaItemAddForm
    {

    }

    // Used to check edit for Media Item
    public class MediaItemEdit : MediaItemAdd
    {
        public int Id { get; set; }
    }

    // Includes basically all the properties (through inheritance in this case)
    public class MediaItemBase : MediaItemAdd
    {
        public int Id { get; set; }
        [Display(Name = "Graded Work Name")]
        public string GradedWorkName { get; set; }
        [Required]
        public byte[] Content { get; set; }
        [Required]
        [Display(Name = "Media Type")]
        public string MediaType { get; set; }
    }
}