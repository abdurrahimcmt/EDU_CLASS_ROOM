using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_Models
{
    public class InfoBatch
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string BatchNo { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        //public string DepartmentId { get; set; }
        
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual DepartmentInfo DepartmentInfo { get; set; }

        
        [Display(Name = "Shift")]
        public int ShiftId { get; set; }
        [ForeignKey("ShiftId")]
        public virtual ShiftInfo ShiftInfo { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
    }
}
