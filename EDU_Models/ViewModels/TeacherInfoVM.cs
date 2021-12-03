using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDU_Models.ViewModels
{
    public class TeacherInfoVM
    {
        public TeacherInfo TeacherInfo { get; set; }
        public IEnumerable<SelectListItem> DesignationSelectList { get; set; }
        public IEnumerable<SelectListItem> UniversitySelectList { get; set; }
    }

}
