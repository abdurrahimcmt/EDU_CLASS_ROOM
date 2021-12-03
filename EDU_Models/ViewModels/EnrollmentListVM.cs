using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_Models.ViewModels
{
    public class EnrollmentListVM
    {
        public EnrollmentVM enrollmentVM { get; set; }

        public IEnumerable<EnrollmentDetails> EnrollmentDetails { get; set; }
    }
}
