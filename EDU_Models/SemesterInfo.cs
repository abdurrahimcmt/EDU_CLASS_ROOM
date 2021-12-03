using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDU_Models
{
    public class SemesterInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
       public string Description { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
    }
}
