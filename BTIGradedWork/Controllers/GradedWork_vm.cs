using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BTIGradedWork.Controllers
{
    // Used to create 'add graded work' form
    public class GradedWorkAddForm
    {
        [Display(Name = "Graded Work Name")]
        public string Name { get; set; }
        [Display(Name = "Total Value")]
        public double TotalValue { get; set; }
        [DisplayFormat(DataFormatString = "{0:D}")]
        [Display(Name = "Date Assigned")]
        public DateTime DateAssign { get; set; }
        [DisplayFormat(DataFormatString = "{0:f}")]
        [Display(Name = "Due Date")]
        public DateTime DateDue { get; set; }
        [DisplayFormat(DataFormatString = "{0:f}")]
        [Display(Name = "Final Due Date")]
        public DateTime DueDateLate { get; set; }
        [Display(Name = "Estimated Work Hours For Student")]
        public int EstWorkHours { get; set; }
        [Display(Name = "Graded Work Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        //public SelectList Courses { get; set; }
        [Display(Name = "Link to Graded Work")]
        public string InfoURL { get; set; }
    }

    // Used to check gathered data from user
    public class GradedWorkAdd
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} can contain up to {1} characters.")]
        [Display(Name = "Graded Work Name")]
        public string Name { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "{0} must be between {1}% and {2}%.")]
        [Display(Name = "Total Value")]
        public double TotalValue { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:D}")]
        [Display(Name = "Date Assigned")]
        public DateTime DateAssign { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:D}")]
        [Display(Name = "Due Date")]
        public DateTime DateDue { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:D}")]
        [Display(Name = "Final Due Date")]
        public DateTime DueDateLate { get; set; }
        [Display(Name = "Estimated Work Hours For Student")]
        [Range(0, 100, ErrorMessage = "{0} has a limit of {2} hours.")]
        public int EstWorkHours { get; set; }
        [Required]
        [Display(Name = "Graded Work Description")]
        [StringLength(500, ErrorMessage = "{0} can contain up to {1} characters.")]
        public string Description { get; set; }
        public int CourseId { get; set; }
        [Display(Name = "Link to Graded Work")]
        public string InfoURL { get; set; }
    }

    // Used to as the edit form for the Graded Work
    public class GradedWorkEditForm : GradedWorkAddForm
    {

    }

    // Used to check edit for Graded Work
    public class GradedWorkEdit : GradedWorkAdd
    {
        public int Id { get; set; }
    }

    // Includes basically all the properties (through inheritance in this case)
    public class GradedWorkBase : GradedWorkAdd
    {
        public int Id { get; set; }
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }

    }

    // Used to view all media items for a piece of graded work
    public class GradedWorkBaseWithMediaItems : GradedWorkBase
    {
        [Display(Name="Media Items")]
        public ICollection<MediaItemBase> MediaItems { get; set; }
    }
}