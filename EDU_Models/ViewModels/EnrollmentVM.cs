using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_Models.ViewModels
{
    public class EnrollmentVM
    {
        public EnrollmentInfo enrollmentInfo { get; set; }

        public IEnumerable<SelectListItem> StudentSelectList { get; set; }
        public IEnumerable<SelectListItem> SemesterSelectList { get; set; }
        public IEnumerable<SelectListItem> DepartmentSelectList { get; set; }

        public IEnumerable<CourseInfoCheckList> CourseList { get; set; }
    }
}
