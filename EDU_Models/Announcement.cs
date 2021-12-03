using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDU_Models
{
    public class Announcement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Massage")]
        public string Massage { get; set; }

        public string filePath { get; set; }
     

        [ForeignKey("UserId")]
        public string UserId { get; set; }
    }
}
