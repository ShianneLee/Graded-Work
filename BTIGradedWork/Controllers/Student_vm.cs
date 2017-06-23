using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BTIGradedWork.Controllers
{
    public class StudentList
    {
        // Used for 'select' options
        public int Id { get; set; }
        public string Name { get; set; }
    }

    // Used to create 'add student' form
    public class StudentAddForm
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Student Number")]
        public string StudentId { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Display(Name = "Program Code")]
        public string ProgramCode { get; set; }
    }

    // Used to check gathered data from user
    public class StudentAdd
    {
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Email address")]
        public string Email { get; set; }
        [Required]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "{0} can contain only {1} numeric digits.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} can contain only numeric digits.")]
        [Display(Name = "Student number")]
        public string StudentId { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "{0} can contain only {2} to {1} characters.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "{0} must contain only alphabetic characters.")]
        [Display(Name = "Program code")]
        public string ProgramCode { get; set; }
        public string UserName { get; set; }
    }

    // Form used to edit student's details
    public class StudentEditForm : StudentAddForm
    {
        public string UserName { get; set; }
    }

    // Used to edit student's details
    public class StudentEdit 
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} can contain up to {1} characters.")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[A-Za-z\-\']*$", ErrorMessage = "{0} must be alphabetic characters with special characters such as hyphen(-) and apostophe(').")]
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
        [Required]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "{0} can contain only {1} numeric digits.")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} can contain only numeric digits.")]
        [Display(Name = "Student Number")]
        public string StudentId { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "{0} can contain only {2} to {1} characters.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "{0} must contain only alphabetic characters.")]
        [Display(Name = "Program Code")]
        public string ProgramCode { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    // Includes basically all the properties (through inheritance in this case)
    public class StudentBase : StudentAdd
    {
        public int Id { get; set; }
        [Display(Name = "Full Name")]
        public string Name { get; set; }
    }

    // Used to view all courses for a student
    public class StudentBaseWithCourses : StudentBase
    {
        public ICollection<CourseBase> Courses { get; set; }
    }
}