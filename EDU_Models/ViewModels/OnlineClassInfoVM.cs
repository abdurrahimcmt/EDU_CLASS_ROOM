using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_Models.ViewModels
{
    public class OnlineClassInfoVM
    {
        public OnlineClassInfo OnlineClassInfo { get; set; }

        public IEnumerable<SelectListItem> CourseSelectList { get; set; }
        public IEnumerable<SelectListItem> SemesterSelectList { get; set; }
        public IEnumerable<SelectListItem> DepartmentSelectList { get; set; }
        public IEnumerable<SelectListItem> TeacherSelectList { get; set; }
        public IEnumerable<StudentInfoCheckList> StudentList { get; set; }
    }
}
