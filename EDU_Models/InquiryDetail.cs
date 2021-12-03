using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDU_Models
{
    public class InquiryDetail
    {
        [Key]

        public int Id { get; set; }

        [Required]
        [ForeignKey("InquiryHeaderId")]
        public int InquiryHeaderId { get; set; }

        public InquiryHeader InquiryHeader { get; set; }

        [Required]
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
