using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_Models.ViewModels
{
    public class StudentInfoCheckList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }
        [Required]
        [Display(Name = "Student Id")]
        public string StudentId { get; set; }
        [Required]
        [Display(Name = "Fathers Name")]
        public string FathersName { get; set; }
        [Required]
        [Display(Name = "Mothers Name")]
        public string MothersName { get; set; }
        [Required]
        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = "Shift")]
        public int ShiftId { get; set; }
        [ForeignKey("ShiftId")]
        public virtual ShiftInfo ShiftInfo { get; set; }


        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual DepartmentInfo DepartmentInfo { get; set; }

        [Display(Name = "Batch Name")]
        public int BatchId { get; set; }
        [ForeignKey("BatchId")]
        public virtual InfoBatch InfoBatch { get; set; }

        
        public string Image { get; set; }
        public string Description { get; set; }
  
        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public SelectListItem InvitedStudent { get; set; }
    }
}
