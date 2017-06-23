using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BTIGradedWork.Controllers
{
    // Used for 'select' option
    public class TeacherList
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // Used to create 'add teacher' form
    public class TeacherAddForm
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Office Number")]
        public string Office { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }

    // Used to check gathered data from user
    public class TeacherAdd
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Email address")]
        public string Email { get; set; }
        [Display(Name = "Office Number")]
        public string Office { get; set; }
        public string UserName { get; set; }
    }

    // Form used to edit teacher's details
    public class TeacherEditForm : TeacherAddForm
    {
        public string UserName { get; set; }
    }

    // Used to edit teacher's details
    public class TeacherEdit
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} can contain up to {1} characters.")]
        [RegularExpression(@"^[A-Za-z\-\']*$", ErrorMessage = "{0} must be alphabetic characters with special characters such as hyphen(-) and apostophe(').")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} can contain up to {1} characters.")]
        [RegularExpression(@"^[A-Za-z\-\']*$", ErrorMessage = "{0} must be alphabetic characters with special characters such as hyphen(-) and apostophe(').")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Display(Name = "Office Number")]
        public string Office { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    // Includes basically all properties(through inheritance in this case)
    public class TeacherBase : TeacherAdd
    {
        public int Id { get; set; }
        [Display(Name = "Full Name")]
        public string Name { get; set; }
    }

    // Used to view all courses for a teacher
    public class TeacherBaseWithCourses : TeacherBase
    {
        public ICollection<CourseBase> Courses { get; set; }
    }
}