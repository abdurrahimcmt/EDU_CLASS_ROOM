using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_Models
{
    public class OnlineClassInfo
    {
        [Key]
        public int Id { get; set; }
        public String Title { get; set; }

        [Display(Name = "Course")]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual CourseInfo courseInfo { get; set; }
        public String CourseName { get; set; }

        [Display(Name = "Semester")]
        public int SemesterId { get; set; }
        [ForeignKey("SemesterId")]
        public virtual SemesterInfo semesterInfo { get; set; }

        public String SemesterName { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual DepartmentInfo departmentInfo { get; set; }
        public String DepartmentName { get; set; }

        [Display(Name = "Teacher")]
        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual TeacherInfo teacherInfo { get; set; }
        public String TeacherName { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
    }
}
