using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;

namespace BTIGradedWork
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Data Store Initializer
            System.Data.Entity.Database.SetInitializer(new Models.StoreInitializer());

            // User
            Mapper.CreateMap<Models.ApplicationUser, Models.ApplicationUserBase>();

            // Student
            Mapper.CreateMap<Controllers.StudentAdd, Models.Student>();
            Mapper.CreateMap<Models.Student, Controllers.StudentBase>();
            Mapper.CreateMap<Controllers.StudentBase, Controllers.StudentBaseWithCourses>();
            Mapper.CreateMap<Controllers.StudentBase, Controllers.StudentEditForm>();
            Mapper.CreateMap<Controllers.StudentEdit, Controllers.StudentEditForm>();

            // Teacher
            Mapper.CreateMap<Controllers.TeacherAdd, Models.Teacher>();
            Mapper.CreateMap<Models.Teacher, Controllers.TeacherBase>();
            Mapper.CreateMap<Controllers.TeacherBase, Controllers.TeacherBaseWithCourses>();
            Mapper.CreateMap<Controllers.TeacherBase, Controllers.TeacherEditForm>();
            Mapper.CreateMap<Controllers.TeacherEdit, Controllers.TeacherEditForm>();

            // Course
            Mapper.CreateMap<Controllers.CourseAdd, Models.Course>();
            Mapper.CreateMap<Models.Course, Controllers.CourseBase>();
            Mapper.CreateMap<Controllers.CourseAdd, Controllers.CourseAddForm>();
            Mapper.CreateMap<Controllers.CourseBase, Controllers.CourseEditForm>();
            Mapper.CreateMap<Controllers.CourseEdit, Controllers.CourseEditForm>();
            Mapper.CreateMap<Models.Course, Controllers.CourseList>();

            // Graded Work
            Mapper.CreateMap<Controllers.GradedWorkAdd, Models.GradedWork>();
            Mapper.CreateMap<Models.GradedWork, Controllers.GradedWorkBase>();
            Mapper.CreateMap<Models.GradedWork, Controllers.GradedWorkBaseWithMediaItems>();
            Mapper.CreateMap<Controllers.GradedWorkBaseWithMediaItems, Controllers.GradedWorkEditForm>();
            Mapper.CreateMap<Controllers.GradedWorkEdit, Controllers.GradedWorkEditForm>();

            // Media Item
            Mapper.CreateMap<Controllers.MediaItemAdd, Models.MediaItem>();
            Mapper.CreateMap<Models.MediaItem, Controllers.MediaItemBase>();
            Mapper.CreateMap<Controllers.MediaItemBase, Models.MediaItem>();
            Mapper.CreateMap<Controllers.MediaItemAdd, Controllers.MediaItemAddForm>();
            Mapper.CreateMap<Controllers.MediaItemAdd, Controllers.MediaItemBase>();
            Mapper.CreateMap<Controllers.MediaItemBase, Controllers.MediaItemEditForm>();
            Mapper.CreateMap<Controllers.MediaItemEdit, Controllers.MediaItemEditForm>();
        }

    }
}
