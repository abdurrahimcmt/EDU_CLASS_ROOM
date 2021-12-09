
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace EDU_Models.ViewModels
{
    public class CourseInfoCheckList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual DepartmentInfo DepartmentInfo { get; set; }

        public string DepartmentName { get; set; }
        public string Description { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public SelectListItem takeCourses { get; set; }

    }
}
