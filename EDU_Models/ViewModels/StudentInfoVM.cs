using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDU_Models.ViewModels
{
    public class StudentInfoVM
    {
        public static object studentinfo;

        public StudentInfo StudentInfo { get; set; }
        public IEnumerable<SelectListItem> DepartmentSelectList { get; set; }
        public IEnumerable<SelectListItem> BatchSelectList { get; set; }
        public IEnumerable<SelectListItem> ShiftSelectList { get; set; }
    }
}
