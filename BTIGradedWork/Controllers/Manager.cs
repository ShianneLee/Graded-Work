using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTIGradedWork.Models;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;

namespace BTIGradedWork.Controllers
{
    public class Manager
    {
        private ApplicationDbContext ds = new ApplicationDbContext();

        // USER 
        // -- Get Users
        public IEnumerable<ApplicationUser> GetUsers()
        {
            // Get all users
            var users = ds.Users.OrderBy(u => u.UserName);
            return users;
        }

        // -- Username Exists
        public int UsernameExists(string username)
        {
            // Counts number of users with username passed in
            var users = ds.Users.Where(u => u.UserName == username).Count();
            return users;
        }

        // -- Email Exists
        public int EmailExists(string email)
        {
            // Counts number of users with email passed in
            var users = ds.Users.Where(u => u.Email == email).Count();
            return users;
        }

        // -- Student Number Exists
        public int StudentIdExists(string studentNum)
        {
            // Counts number of students with student number passed in
            var users = ds.Students.Where(u => u.StudentID == studentNum).Count();
            return users;
        }

        // -- Get User Info
        public ApplicationUserBase GetUserInfo(string userName)
        {
            // Create user manager object
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ds));

            // Attempt to fetch the user object
            var user = userManager.FindByName(userName);

            // Check if user exists
            if (user == null)
            {
                return null;
            }
            else
            {
                // Prepare view model object
                var appUser = Mapper.Map<ApplicationUserBase>(user);
                // Add role names
                foreach (var role in user.Roles)
                {
                    appUser.RolesForUser.Add(role.Role.Name);
                }
                return appUser;
            }
        }

        // -- Get Roles
        //public  GetRoles()
        //{
            
        //}

        // -- Remove User
        public bool RemoveUser(string username)
        {
            // Fetch user
            var fetched = ds.Users.Find(username);

            // Check if user exists
            if (fetched == null)
                return false;
            else {
                // Remove user
                ds.Users.Remove(fetched);
                ds.SaveChanges();
                return true;
            }
        }

        // STUDENT
        // -- Get All Students
        public IEnumerable<StudentBase> GetAllStudents()
        {
            // Fetch students
            var fetched = ds.Students.OrderByDescending(s => s.Id);
            return Mapper.Map<IEnumerable<StudentBase>>(fetched);
        }

        // -- Get All Students With Courses
        public StudentBaseWithCourses GetStudentWithCourses(int id)
        {
            // Fetch student with their courses
            var fetched = ds.Students.Include("Course").SingleOrDefault(s => s.Id == id);
            return Mapper.Map<StudentBaseWithCourses>(fetched);
        }

        // -- Get Student with Course list; sorted
        public IEnumerable<CourseList> GetStudentCourseList()
        {
            var username = HttpContext.Current.User.Identity.Name;

            var fetched = ds.Students.Include("Courses").SingleOrDefault(u => u.UserName == username);

            return Mapper.Map<IEnumerable<CourseList>>(fetched.Courses);
        }

        // -- Get Student's Full Name
        public string GetStudentName(int id)
        {
            // Fetch student
            var fetched = ds.Students.FirstOrDefault(i => i.Id == id);

            // Check if student exists
            if (fetched == null)
                return null;
            else
            {
                // Create full name from first and last name
                string fullname = fetched.FirstName + " " + fetched.LastName;
                return fullname;
            }
        }

        // -- Add Student
        public StudentBase AddStudent(StudentAdd newItem)
        {
            // Map StudentAdd object (newItem) to Student object (newStudent)
            var newStudent = Mapper.Map<Student>(newItem);

            // Add new Student object to data store and save changes
            ds.Students.Add(newStudent);
            ds.SaveChanges();

            // Map Student object (newStudent) to StudentBase object (studentBase) and pass the identifier
            var studentBase = Mapper.Map<StudentBase>(newStudent);
            studentBase.Id = newStudent.Id;

            // Return StudentBase object
            return studentBase;
        }

        // -- Get Student By Id
        public StudentBase GetStudentById(int id)
        {
            // Get specific Student object from data store using identifier
            var fetched = ds.Students.Find(id);

            // Check if it exists
            if (fetched == null)
                return null;
            else
            {
                StudentBase studB = Mapper.Map<StudentBase>(fetched);
                studB.Name = GetStudentName(id);
                // Returns StudentBase object
                return studB;
            }
        }

        //// -- Get Student By Id With Courses
        //public StudentBaseWithCourses GetStudentWithCourses(int id)
        //{
        //    // Get specific Student object from data store using identifier
        //    var fetched = ds.Students.Include("Courses").First(s => s.Id == id);

        //    // Check if it exists
        //    if (fetched == null)
        //        return null;
        //    else
        //    {
        //        var studBC = Mapper.Map<StudentBaseWithCourses>(fetched);
        //        studBC.Name = GetStudentName(id);
        //        // Returns StudentBase object
        //        return studBC;
        //    }
        //}

        // -- Edit Student
        public StudentBase EditStudent(StudentEdit student)
        {
            // Get specific Student object from data store to be edited
            var fetched = ds.Students.Find(student.Id);
            //var alsoFetched = ds.Users.Find(student.UserName);

            // Check if it exists
            if (fetched == null)
                return null;
            else
            {
                // Change values that are different
                // for students
                //ds.Entry(fetched).CurrentValues.SetValues(student);
                fetched.Email = student.Email;
                fetched.FirstName = student.FirstName;
                fetched.LastName = student.LastName;
                fetched.ProgramCode = student.ProgramCode;
                fetched.StudentID = student.StudentId;
                ds.SaveChanges();

                // for users
                var updated = UpdateStudentUser(student);
                if (updated == false)
                    return null;

                // Student object (fetched) to StudentBase object (edited)
                var edited = Mapper.Map<StudentBase>(fetched);
                edited.Id = fetched.Id;
                return edited;
            }
        }

        // -- Update Student User
        public bool UpdateStudentUser(StudentEdit toBeEdited)
        {
            // Fetch user
            var fetched = ds.Users.Find(toBeEdited.UserName);

            // Check if user exists
            if (fetched == null)
                return false;
            else
            {
                // Update data
                fetched.Email = toBeEdited.Email;
                fetched.FirstName = toBeEdited.FirstName;
                fetched.LastName = toBeEdited.LastName;
                ds.SaveChanges();
                return true;
            }
        }

        // -- Delete Student
        public bool DeleteStudent(int id)
        {
            // Fetch student by ID
            var fetched = ds.Students.Find(id);

            // Check if student exists
            if (fetched == null)
                return false;
            else
            {
                // Remove student
                ds.Students.Remove(fetched);
                ds.SaveChanges();
                // Remove student as user
                if (RemoveUser(fetched.UserName) == false)
                {
                    return false;
                }
                return true;
            }
        }

        // TEACHER
        // -- Get All Teachers
        public IEnumerable<TeacherBase> GetAllTeachers()
        {
            // Fetch teachers
            var fetched = ds.Teachers.OrderByDescending(s => s.Id);
            return Mapper.Map<IEnumerable<TeacherBase>>(fetched);
        }
        // -- Get All Teachers As List
        public IEnumerable<TeacherList> GetAllTeachersAsList()
        {
            // Fetch teachers
            var fetched = ds.Teachers.OrderBy(t => t.FirstName);

            //Create teacher list
            var teachers = new List<TeacherList>();
            foreach (var item in fetched)
            {
                var t = new TeacherList();
                t.Id = item.Id;
                t.Name = GetTeacherName(item.Id);
                teachers.Add(t);
            }
            return teachers;
        }

        // -- Get Teacher with Course list; sorted
        public IEnumerable<CourseList> GetTeacherCourseList()
        {
            var username = HttpContext.Current.User.Identity.Name;

            var fetched = ds.Teachers.Include("Courses").SingleOrDefault(u => u.UserName == username);

            return Mapper.Map<IEnumerable<CourseList>>(fetched.Courses);
        }

        // -- Get Teacher's Full Name
        public string GetTeacherName(int id)
        {
            // Find teacher object
            var fetched = ds.Teachers.FirstOrDefault(t => t.Id == id);

            // Check if teacher exists
            if (fetched == null)
            {
                return null;
            }
            else
            {
                // Creates a full name using first and last name
                string fullname = fetched.FirstName + " " + fetched.LastName;
                return fullname;
            }
        }

        // -- Add Teacher
        public TeacherBase AddTeacher(TeacherAdd newItem)
        {
            // Map TeacherAdd object (newItem) to Teacher object (newTeacher)
            var newTeacher = Mapper.Map<Teacher>(newItem);

            // Add new Teacher object to data store and save changes
            ds.Teachers.Add(newTeacher);
            ds.SaveChanges();

            // Map Teacher object (newTeacher) to TeacherBase object (teacherBase) and pass the identifier
            var teacherBase = Mapper.Map<TeacherBase>(newTeacher);
            teacherBase.Id = newTeacher.Id;

            // Return TeacherBase object
            return teacherBase;
        }

        // -- Get Teacher By Id
        public TeacherBase GetTeacherById(int id)
        {
            // Get specific Teacher object from data store using identifier
            var fetched = ds.Teachers.Find(id);

            // Check if it exists
            if (fetched == null)
                return null;
            else
            {
                TeacherBase teacherB = Mapper.Map<TeacherBase>(fetched);
                teacherB.Name = GetTeacherName(id);
                var teacherBC = Mapper.Map<TeacherBaseWithCourses>(teacherB);
                // Returns TeacherBase object
                return teacherBC;
            }
        }

        // -- Edit Teacher
        public TeacherBase EditTeacher(TeacherEdit teacher)
        {
            // Get specific Teacher object from data store to be edited
            var fetched = ds.Teachers.Find(teacher.Id);
            //var alsoFetched = ds.Users.Find(teacher.UserName);

            // Check if it exists
            if (fetched == null)
                return null;
            else
            {
                // Change values that are different
                // for teachers
                //ds.Entry(fetched).CurrentValues.SetValues(teacher);
                fetched.Email = teacher.Email;
                fetched.FirstName = teacher.FirstName;
                fetched.LastName = teacher.LastName;
                fetched.Office = teacher.Office;
                ds.SaveChanges();

                // for users
                var updated = UpdateTeacherUser(teacher);
                if (updated == false)
                    return null;

                // Teacher object (fetched) to TeacherBase object (edited)
                var edited = Mapper.Map<TeacherBase>(fetched);
                edited.Id = fetched.Id;
                return edited;
            }
        }

        // -- Update Teacher User
        public bool UpdateTeacherUser(TeacherEdit toBeEdited)
        {
            // Fetch User
            var fetched = ds.Users.Find(toBeEdited.UserName);

            // Check if user exists
            if (fetched == null)
                return false;
            else
            {
                // Update data
                fetched.Email = toBeEdited.Email;
                fetched.FirstName = toBeEdited.FirstName;
                fetched.LastName = toBeEdited.LastName;
                ds.SaveChanges();
                return true;
            }
        }

        // -- Delete Teacher
        public bool DeleteTeacher(int id)
        {
            // Fetch teachers
            var fetched = ds.Teachers.Find(id);

            // Check if teacher exists
            if (fetched == null)
                return false;
            else
            {
                // Remove if teacher exists
                ds.Teachers.Remove(fetched);
                
                ds.SaveChanges();
                // Remove same teacher as user
                if (RemoveUser(fetched.UserName) == false)
                {
                    return false;
                }
                return true;
            }
        }

        // COURSE
        // -- Get All Courses
        public IEnumerable<CourseBase> GetAllCourses()
        {
            // Fetch courses
            var fetched = ds.Courses.OrderBy(c => c.Code);
            return Mapper.Map<IEnumerable<CourseBase>>(fetched);
        }

        // -- Get All Courses With Teacher
        public IEnumerable<CourseBase> GetAllCoursesWithTeacher()
        {
            var fetched = ds.Courses.Include("Teacher").OrderBy(c => c.Code);

            return Mapper.Map<IEnumerable<CourseBase>>(fetched);
            //// Create a CourseBase object list
            //var courses = new List<CourseBase>();

            //foreach (var course in fetched)
            //{
            //    var c = Mapper.Map<CourseBase>(course);
            //    c.TeacherName = GetTeacherName(course.Teacher.Id);
            //    //// Create CourseBase object to put in list
            //    //var c = new CourseBase()
            //    //{
            //    //    Id = course.Id,
            //    //    Name = course.Name,
            //    //    Code = course.Code,
            //    //    Description = course.Description,
            //    //    OutlineUrl = course.OutlineUrl,
            //    //    Section = course.Section,
            //    //    Semester = course.Semester,
            //    //    TeacherId = course.Teacher.Id,
            //    //    TeacherName = GetTeacherName(course.Teacher.Id)
            //    //};
            //    courses.Add(c);
            //}
            //return courses;
        }

        // -- Get All Courses As List
        public IEnumerable<CourseList> GetAllCoursesAsList()
        {
            // Fetch course code
            var fetched = ds.Courses.OrderBy(c => c.Code);

            // Empty course list
            var courses = new List<CourseList>();

            // Adds courses to list
            foreach (var item in fetched)
            {
                var c = new CourseList();
                c.Id = item.Id;
                c.Code = item.Code;
                courses.Add(c);
            }
            return courses;
        }

        // -- Get Course By Id
        public CourseBase GetCourseById(int id)
        {
            // Fetch course by id
            var fetched = ds.Courses.Include("Teacher").First(c => c.Id == id);
            var cBase = Mapper.Map<CourseBase>(fetched);
            cBase.TeacherName = GetTeacherName(fetched.Teacher.Id);
            return cBase;
        }

        // -- Get Course With Graded Works
        public CourseBaseWithGradedWorks GetCourseWithGradedWorks(int id)
        {
            // Fetch Course with graded works
            var fetched = ds.Courses.Include("GradedWorks").First(c => c.Id == id);

            // Check if course exists
            if (fetched == null)
                return null;
            else
                // Return course base object
                return Mapper.Map<CourseBaseWithGradedWorks>(fetched);
        }

        //// -- Get Course Selection List
        //public List<string> GetCourseSelectionList()
        //{
        //    // Fetch course code
        //    var fetched = ds.Courses.OrderBy(c => c.Code);

        //    // Empty course list
        //    var courses = new List<string>();

        //    // Adds courses to list
        //    foreach (var item in fetched)
        //    {
        //        var c = new string
        //        c.Id = item.Id;
        //        c.Code = item.Code;
        //        courses.Add(c);
        //    }
        //    return courses;
        //}
        // -- Add Course
        public CourseBase AddCourse(CourseAdd toBeAdded)
        {
            // Fetch teacher
            var t = ds.Teachers.Find(toBeAdded.TeacherId);

            // Check if teacher exists
            if (t == null)
            {
                return null;
            }
            else
            {
                if (String.IsNullOrEmpty(toBeAdded.OutlineUrl))
                {
                    toBeAdded.OutlineUrl = "No link available";
                }
                // Add course to data store
                var course = Mapper.Map<Course>(toBeAdded);
                course.Teacher = t;
                ds.Courses.Add(course);
                ds.SaveChanges();

                // Make course object a course base object and return it
                var cBase = Mapper.Map<CourseBase>(course);
                cBase.Id = course.Id;
                //cBase.TeacherId = toBeAdded.TeacherId;
                //cBase.TeacherName = GetTeacherName(toBeAdded.TeacherId);
                return cBase;
            }
        }

        // -- Edit Course
        public CourseBase EditCourse(CourseEdit toBeEdited)
        {
            // Fetch course to be edited
            var fetched = ds.Courses.Find(toBeEdited.Id);

            // Check if course exists
            if (fetched == null)
                return null;
            else
            {
                if (String.IsNullOrEmpty(toBeEdited.OutlineUrl) || toBeEdited.OutlineUrl == "No link available")
                {
                    toBeEdited.OutlineUrl = "No link available";
                }
                // Update values
                ds.Entry(fetched).CurrentValues.SetValues(toBeEdited);
                ds.SaveChanges();

                // Return edited object
                var edited = Mapper.Map<CourseBase>(fetched);
                edited.Id = fetched.Id;
                return edited;
            }
        }

        // -- Delete Course
        public bool DeleteCourse(int id)
        {
            // Fetch object to be deleted
            var fetched = ds.Courses.Find(id);

            // Check if object exists
            if (fetched == null)
                return false;
            else
            {
                // Remove course object from data store
                ds.Courses.Remove(fetched);
                ds.SaveChanges();
                return true;
            }
        }

        // GRADED WORK
        // -- Get All Graded Works
        public IEnumerable<GradedWorkBase> GetAllGradedWorks()
        {
            // Fetch graded works 
            var fetched = ds.GradedWorks.Include("Course").OrderBy(g => g.DateDue);
            return Mapper.Map<IEnumerable<GradedWorkBase>>(fetched);
        }

        // -- Get All Graded Works; sorted
        public IEnumerable<GradedWorkBase> GetAllGradedWorks(int id)
        {
            // Fetch graded works 
            var fetched = ds.GradedWorks.Include("Course").Where(i => i.Course.Id == id).OrderBy(g => g.DateDue);
            return Mapper.Map<IEnumerable<GradedWorkBase>>(fetched);
        }

        //// -- Get All Courses With Graded Work
        //public IEnumerable<GradedWorkBase> GetAllGradedWorksWithCourse()
        //{
        //    var fetched = ds.GradedWorks.Include("Course").OrderBy(g => g.DateDue);

        //    return Mapper.Map<IEnumerable<GradedWorkBase>>(fetched);
        //}

        // -- Get Graded Work By Id
        public GradedWorkBaseWithMediaItems GetGradedWorkById(int id)
        {
            // Fetch Graded Works with Media items
            var fetched = ds.GradedWorks.Include("MediaItems").Include("Course").First(g => g.Id == id);
            // Check if graded work exists
            if (fetched == null)
            {
                return null;
            }
            else
            {
                // Return Graded work base object
                return Mapper.Map<GradedWorkBaseWithMediaItems>(fetched);
            }
        }

        //// -- Get Graded Work By Id
        //public GradedWorkBaseWithMediaItems GetGradedWorkById(int id)
        //{
        //    // Fetch Graded Works with Media items
        //    var fetched = ds.GradedWorks.Include("MediaItem").SingleOrDefault();
        //    return Mapper.Map<GradedWorkBaseWithMediaItems>(fetched);
        //}

        // -- Get Graded Work Name
        public string GetGradedWorkName(int id)
        {
            var fetched = ds.GradedWorks.Find(id);

            return fetched.Name;
        }

        // -- Add Graded Work
        public GradedWorkBase AddGradedWork(GradedWorkAdd toBeAdded)
        {
            // Find course object
            var fetched = ds.Courses.Find(toBeAdded.CourseId);

            //if (newItem.InfoURL == null)
            if (String.IsNullOrEmpty(toBeAdded.InfoURL))
            {
                toBeAdded.InfoURL = "No link available";
            }
            // GradedWorkAdd object (toBeAdded) to GradedWork object(gradedwork)
            var gradedwork = Mapper.Map<GradedWork>(toBeAdded);
            gradedwork.Course = fetched;

            // Add GradedWork object to data store
            ds.GradedWorks.Add(gradedwork);
            ds.SaveChanges();
            
            // GradedWork object to GradedWorkBase object and return base object
            var gwBase = Mapper.Map<GradedWorkBase>(gradedwork);
            gwBase.Id = gradedwork.Id;
            return gwBase;
            //return Mapper.Map<GradedWorkBase>(gradedwork);
        }

        // -- Edit Graded Work
        public GradedWorkBase EditGradedWork(GradedWorkEdit toBeEdited)
        {
            // Fetch graded work
            var fetched = ds.GradedWorks.Find(toBeEdited.Id);

            // Check if it exists
            if (fetched == null)
                return null;
            else
            {
                if (String.IsNullOrEmpty(toBeEdited.InfoURL) || toBeEdited.InfoURL == "No link available")
                {
                    toBeEdited.InfoURL = "No link available";
                }
                // Change values that are different
                ds.Entry(fetched).CurrentValues.SetValues(toBeEdited);
                ds.SaveChanges();

                // GradedWork object (fetched) to GradedWorkBase object (edited)
                var edited = Mapper.Map<GradedWorkBase>(fetched);
                edited.Id = fetched.Id;
                return edited;
            }
        }

        // -- Deleted Graded Work
        public bool DeleteGradedWork(int id)
        {
            // Fetch graded work
            var fetched = ds.GradedWorks.Find(id);

            // Check if graded work exists
            if (fetched == null)
                return false;
            else
            {
                // Remove graded work
                ds.GradedWorks.Remove(fetched);
                ds.SaveChanges();
                return true;
            }
        }

        // MEDIA ITEM
        // -- Get All Media Items
        public IEnumerable<MediaItemBase> GetAllMediaItems()
        {
            // Fetch Media Items
            var fetched = ds.MediaItems.Include("GradeWork").OrderBy(m => m.Id);
            var items = new List<MediaItemBase>();

            foreach (var item in fetched)
            {
                var mi = new MediaItemBase();
                mi.Description = item.Description;
                mi.Id = item.Id;
                mi.Name = item.Name;
                mi.MediaType = item.MediaType;
                mi.GradedWorkId = item.GradeWork.Id;
                mi.GradedWorkName = item.GradeWork.Name;
                items.Add(mi);
            }
            return items;
        }

        // -- Get All Media Items; Sorted
        public IEnumerable<MediaItemBase> GetAllMediaItems(int id)
        {
            // Fetch Media Items
            var fetched = ds.MediaItems.Include("GradeWork").Where(i => i.GradeWork.Id == id).OrderBy(m => m.Id);
            var items = new List<MediaItemBase>();

            foreach (var item in fetched)
            {
                var mi = new MediaItemBase();
                mi.Description = item.Description;
                mi.Id = item.Id;
                mi.Name = item.Name;
                mi.MediaType = item.MediaType;
                mi.GradedWorkId = item.GradeWork.Id;
                mi.GradedWorkName = item.GradeWork.Name;
                items.Add(mi);
            }
            return items;
        }

        // -- Get Media Item By Id
        public MediaItemBase GetMediaItemById(int id)
        {
            var mi = ds.MediaItems.Include("GradeWork").SingleOrDefault(m => m.Id == id);

            // Check if Media Item object exists
            if (mi == null)
            {
                return null;
            }
            else
            {
                // Returns Media Item Base object
                var miBase = Mapper.Map<Controllers.MediaItemBase>(mi);
                miBase.GradedWorkId = mi.GradeWork.Id;
                miBase.GradedWorkName = GetGradedWorkName(mi.GradeWork.Id);
                return (miBase);
            }
        }

        // -- Get Media Item
        public MediaItemBase GetMediaItem(int id)
        {
            var mi = ds.MediaItems.Include("GradeWork").First(m => m.Id == id);
            var miB = Mapper.Map<Controllers.MediaItemBase>(mi);
            miB.GradedWorkName = mi.GradeWork.Name;
            // Check if Media Item object exists
            if (mi == null)
            {
                return null;
            }
            else
            {
                // Returns Media Item Base object
                return (miB);
            }
        }

        // -- Get Upload
        public MediaItemBase GetUpload(int id)
        {
            // Fetch media item
            var mi = ds.MediaItems.Find(id);

            // Check if item exists
            if (mi == null)
                return null;
            else
                // Return Media Base object
                return Mapper.Map<MediaItemBase>(mi);
        }

        // -- Add Media Item
        public MediaItemBase AddMediaItem(MediaItemAdd toBeAdded)
        {
            var mediaBase = Mapper.Map<MediaItemBase>(toBeAdded);
            
            // Get uploaded file and assign to media item object
            byte[] ItemBytes = new byte[toBeAdded.ItemUpload.ContentLength];
            toBeAdded.ItemUpload.InputStream.Read(ItemBytes, 0, toBeAdded.ItemUpload.ContentLength);
            mediaBase.Content = ItemBytes;
            mediaBase.MediaType = toBeAdded.ItemUpload.ContentType;

            // Find the graded work object
            var gradedwork = ds.GradedWorks.Find(toBeAdded.GradedWorkId);

            // Add Media Item object to data store
            var media = Mapper.Map<MediaItem>(mediaBase);
            media.GradeWork = gradedwork;
            ds.MediaItems.Add(media);
            ds.SaveChanges();

            mediaBase.Id = media.Id;
            mediaBase.GradedWorkName = GetGradedWorkName(toBeAdded.GradedWorkId);
            //// Add Media Item object to Graded Work object
            //var gradedwork = ds.GradedWorks.Find(toBeAdded.GradedWorkId);
            //gradedwork.MediaItems.Add(media);
            //ds.SaveChanges();

            //mediaBase.GradedWorkId = toBeAdded.GradedWorkId;
            //mediaBase.GradedWorkName = GetGradedWorkName(toBeAdded.GradedWorkId);
            // Return Media Item Base
            return mediaBase;
        }

        // -- Edit Media Item
        public MediaItemBase EditMediaItem(MediaItemEdit toBeEdited)
        {
            // Fetch media item
            var fetched = ds.MediaItems.Find(toBeEdited.Id);

            // Check if item exists
            if (fetched == null)
                return null;
            else
            {
                // Change values that are different
                ds.Entry(fetched).CurrentValues.SetValues(toBeEdited);
                ds.SaveChanges();

                // Media Item object to MediaItemBase object
                var edited = Mapper.Map<MediaItemBase>(fetched);
                edited.Id = fetched.Id;
                return edited;
            }
        }

        // -- Delete Media Item
        public bool DeleteMediaItem(int id)
        {
            // Fetch graded work
            var fetched = ds.MediaItems.Find(id);

            // Check if graded work exists
            if (fetched == null)
                return false;
            else
            {
                // Remove graded work
                ds.MediaItems.Remove(fetched);
                ds.SaveChanges();
                return true;
            }
        }
    }
}