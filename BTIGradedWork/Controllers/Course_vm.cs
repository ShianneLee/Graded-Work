using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BTIGradedWork.Controllers
{
    // Used for 'select' options
    public class CourseList
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }

    //public class CourseSelect : CourseList
    //{
    //    public ICollection<string> Courses { get; set; }
    //}

    // Used to create 'add course' form
    public class CourseAddForm
    {
        [Display(Name = "Course Name")]
        public string Name { get; set; }
        [Display(Name = "Course Code")]
        public string Code { get; set; }
        [Display(Name = "Brief Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Course Outline URL")]
        public string OutlineUrl { get; set; }
        public string Section { get; set; }
        public SelectList Teachers { get; set; }
        public string Semester { get; set; }
    }

    // Used to check gathered data from user
    public class CourseAdd
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} can contain up to {1} characters.")]
        [RegularExpression(@"^[A-Za-z\-\'()&\s]*$", ErrorMessage = "{0} must be alphabetic characters.")]
        [Display(Name = "Course Name")]
        public string Name { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "{0} can be between {1} and {2} characters.")]
        [RegularExpression(@"^[A-Za-z]{3}[0-9]{3}$", ErrorMessage = "{0} must be 3 alphabetic characters and 3 numeric characters.")]
        [Display(Name = "Course Code")]
        public string Code { get; set; }
        [Display(Name = "Brief Description")]
        [StringLength(500, ErrorMessage = "{0} can contain up to {1} characters.")]
        public string Description { get; set; }
        [Display(Name = "Course Outline URL")]
        public string OutlineUrl { get; set; }
        [StringLength(2, ErrorMessage = "{0} can contain up to {1} characters.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "{0} must be alphabetic characters.")]
        public string Section { get; set; }
        public int TeacherId { get; set; }
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "{0} must be alphabetic characters.")]
        public string Semester { get; set; }
    }

    // Form used to edit course's details
    public class CourseEditForm : CourseAddForm
    {

    }

    // Used to edit course's details
    public class CourseEdit : CourseAdd
    {
        public int Id { get; set; }
    }

    // Includes basically all the properties (through inheritance in this case)
    public class CourseBase : CourseAdd
    {
        public int Id { get; set; }
        [Display(Name = "Teacher Name")]
        public string TeacherName { get; set; }
    }

    // Used to view all students for a course
    public class CourseBaseWithStudents : CourseBase
    {
        public ICollection<StudentBase> Students { get; set; }
    }

    // Used to view all graded works for a course
    public class CourseBaseWithGradedWorks : CourseBase
    {
        public ICollection<GradedWorkBase> GradedWorks { get; set; }
    }
}