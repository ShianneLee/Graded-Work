using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CsvHelper;
using BTIGradedWork.Controllers;
using System.Web.Security;

namespace BTIGradedWork.Models
{
    public class StoreInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        private Manager m = new Manager();
        protected override void Seed(ApplicationDbContext context)
        {
            // USER 
            if (context.Users.Count() == 0)
            {
                // Configure the startup users and roles

                // Create a user manager object
                var userManager =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // Create a role manager object
                var roleManager =
                    new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                // Create roles
                foreach (var role in new List<string>() { "Administrator", "Teacher", "Student" })
                {
                    roleManager.Create(new IdentityRole(role));
                }

                // Create users

                var admin = new ApplicationUser()
                {
                    FirstName = "App",
                    LastName = "Administrator",
                    Email = "admin@example.com",
                    UserName = "admin"
                };
                userManager.Create(admin, "password!");
                userManager.AddToRole(admin.Id, "Administrator");

                context.SaveChanges();
            }
            
            // STUDENT CSV
            if (context.Students.Count() == 0)
            {
                // Create a user manager object
                var userManager =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // Create a role manager object
                var roleManager =
                    new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                //File system path to the data file
                string path = HttpContext.Current.Server.MapPath("~/App_Data/ict-students.csv");

                // Create a stream reader object to read the file stream
                StreamReader sr = File.OpenText(path);

                // Create the csvHelper object
                var csv = new CsvReader(sr);
                csv.Configuration.WillThrowOnMissingField = false;

                // Go through the data file
                while (csv.Read())
                {
                    // Create a StudentAdd object from the line of text
                    StudentAdd toBeAdded = csv.GetRecord<StudentAdd>();
                    // Create a StudentBase object(newStudent) from StudentAdd object(toBeAdded)
                    StudentBase newStudent = m.AddStudent(toBeAdded);

                    // Create Application User object from StudentBase object(newStudent)
                    var user = new ApplicationUser()
                    {
                        UserName = newStudent.UserName,
                        FirstName = newStudent.FirstName,
                        LastName = newStudent.LastName,
                        Email = newStudent.Email
                    };

                    userManager.Create(user, "password!");
                    userManager.AddToRole(user.Id, "Student");
                }
                context.SaveChanges();

                // Clean up 
                sr.Close();
                sr = null;
            }

            // SUBJECT/COURSE CSV
            if (context.Courses.Count() == 0)
            {
            // File system path to the data file
            string path2 = HttpContext.Current.Server.MapPath("~/App_Data/ict-subjects-bsd.csv");

            // Create a stream reader object to read the file stream
            StreamReader sr2 = File.OpenText(path2);

            // Create the csvHelper object
            var csv2 = new CsvReader(sr2);
            csv2.Configuration.WillThrowOnMissingField = false;

            // Go through the data file
            while (csv2.Read())
            {
                // Create an object from the line of text
                CourseAdd newCourse = csv2.GetRecord<CourseAdd>();

                // Add it to the data store
                context.Courses.Add(Mapper.Map<Course>(newCourse));
            }
            context.SaveChanges();

            // Clean up 
            sr2.Close();
            sr2 = null;
            }

            // TEACHER CSV
            if (context.Teachers.Count() == 0)
            {
                // Create a user manager object
                var userManager =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // Create a role manager object
                var roleManager =
                    new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                // File system path to the data file
                string path = HttpContext.Current.Server.MapPath("~/App_Data/ict-teachers.csv");

                // Create a stream reader object to read the file stream
                StreamReader sr = File.OpenText(path);

                // Create the csvHelper object
                var csv = new CsvReader(sr);
                csv.Configuration.WillThrowOnMissingField = false;

                // Go through the data file
                while (csv.Read())
                {
                    // Create a TeacherAdd object from the line of text
                    TeacherAdd toBeAdded = csv.GetRecord<TeacherAdd>();
                    // Create a TeacherBase object(newTeacher) from TeacherAdd object(toBeAdded)
                    TeacherBase newTeacher = m.AddTeacher(toBeAdded);

                    // Create Application User object from TeacherBase object(newTeacher)
                    var user = new ApplicationUser()
                    {
                        UserName = newTeacher.UserName,
                        FirstName = newTeacher.FirstName,
                        LastName = newTeacher.LastName,
                        Email = newTeacher.Email
                    };
                    userManager.Create(user, "password!");
                    userManager.AddToRole(user.Id, "Teacher");
                    
                }
                context.SaveChanges();

                // Clean up 
                sr.Close();
                sr = null;
            }

            
        }
    }
}