using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDU_Models.ViewModels
{
    public class AdminInfoVM
    {
        public AdminInfo adminInfo { get; set; }
        public IEnumerable<SelectListItem> DesignationSelectList { get; set; }
    }
}
