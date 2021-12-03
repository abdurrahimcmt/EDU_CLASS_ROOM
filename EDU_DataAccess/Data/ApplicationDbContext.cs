using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EDU_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDU_DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

       public DbSet<Category> Category { get; set; }

        public DbSet<DepartmentInfo> DepartmentInfo { get; set; }
        public DbSet<Announcement> Announcement { get; set; }
        public DbSet<InfoBatch> InfoBatch { get; set; }
        public DbSet<ShiftInfo> ShiftInfo { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<UniversityName> UniversityName { get; set; }
        public DbSet<TeacherInfo> TeacherInfo { get; set; }
        public DbSet<AdminInfo> AdminInfo { get; set; }
        public DbSet<SemesterInfo> SemesterInfo { get; set; }
        public DbSet<CourseInfo> CourseInfo { get; set; }

        public DbSet<EnrollmentInfo> EnrollmentInfo { get; set; }
        public DbSet<EnrollmentDetails> EnrollmentDetails { get; set; }
        public DbSet<StudentInfo> StudentInfo { get; set; }
        public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<InquiryHeader> InquiryHeader { get; set; }

       public DbSet<InquiryDetail> InquiryDetail { get; set; }

       public DbSet<OrderHeader> OrderHeader { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }
    }
}
