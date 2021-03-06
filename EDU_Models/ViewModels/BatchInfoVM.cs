using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDU_Models.ViewModels
{
    public class BatchinfoVM
    {
        public InfoBatch InfoBatch { get; set; }
        public IEnumerable<SelectListItem> DepartmentSelectList { get; set; }
        public IEnumerable<SelectListItem> ShiftInfoSelectList { get; set; }

    }
}
