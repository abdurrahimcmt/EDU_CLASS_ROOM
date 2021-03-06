using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_Models
{
    public class EnrollmentInfo
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Student Id")]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual StudentInfo studentInfo { get; set; }

        public String StudentRoll { get; set; }
        public String StudentName { get; set; }

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


        [ForeignKey("UserId")]
        public string UserId { get; set; }
    }
}
