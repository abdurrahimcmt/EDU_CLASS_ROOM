using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_Models
{
    public class OnlineClassDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ClassId")]
        public int ClassId { get; set; }

        public OnlineClassInfo OnlineClassInfo { get; set; }

        public String ClassTitle { get; set; }


        [Required]
        [ForeignKey("BatchId")]
        public int BatchId { get; set; }

        public CourseInfo BatchInfo { get; set; }

        [Required]
        [ForeignKey("StudentId")]
        public int StudentId { get; set; }

        public StudentInfo StudentInfo { get; set; }

        public String StudentRoll { get; set; }
        public String StudentName { get; set; }
    }
}
