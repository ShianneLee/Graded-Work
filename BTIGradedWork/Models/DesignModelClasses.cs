using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BTIGradedWork.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [StringLength(9)]
        public string StudentID { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string ProgramCode { get; set; }
        public string UserName { get; set; }
        public ICollection<Course> Courses { get; set; }
        public Student()
        {
            this.Courses = new List<Course>();
        }
    }

    public class Teacher
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        public string Office { get; set; }
        [Required]
        public string Email { get; set; }
        public string UserName { get; set; }
        public ICollection<Course> Courses { get; set; }
        public Teacher()
        {
            this.Courses = new List<Course>();
        }
    }

    public class Course
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(6)]
        public string Code { get; set; }
        public string Description { get; set; }
        public string OutlineUrl { get; set; }
        [StringLength(2)]
        public string Section { get; set; }
        public string Semester { get; set; }
        public Teacher Teacher { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<GradedWork> GradedWorks { get; set; }
        public Course()
        {
            this.Students = new List<Student>();
            this.GradedWorks = new List<GradedWork>();
        }
    }

    public class GradedWork
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public double TotalValue { get; set; } // Value of graded work
        [Required]
        public DateTime DateDue { get; set; }
        [Required]
        public DateTime DateAssign { get; set; }
        [Required]
        public DateTime DueDateLate { get; set; } // Due date with reduced value; defaults to due date
        public int EstWorkHours { get; set; } // Work in hours
        [Required]
        public string Description { get; set; }
        public string InfoURL { get; set; }
        public Course Course { get; set; }
        public ICollection<MediaItem> MediaItems { get; set; }
        public GradedWork()
        {
            this.MediaItems = new List<MediaItem>();
            this.TotalValue = 0.00;
            //this.DateAssign = DateTime.Now;
            //this.DateDue = DateTime.Today.AddDays(7);
            //this.DueDateLate = DateTime.Today.AddDays(7);
            this.EstWorkHours = 0;
        }
    }

    public class MediaItem
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public byte[] Content { get; set; }
        [Required]
        public string MediaType { get; set; }
        public GradedWork GradeWork { get; set; }
    }
}