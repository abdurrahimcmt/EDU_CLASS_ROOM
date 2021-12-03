using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_Models
{
    public class EnrollmentDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("EnrollmentId")]
        public int EnrollmentId { get; set; }

        public EnrollmentInfo EnrollmentInfo { get; set; }



        [Required]
        [ForeignKey("CourseId")]
        public int CourseId { get; set; }

        public CourseInfo CourseInfo { get; set; }

        [NotMapped]
        public bool isSelect { get; set; }
    }
}
