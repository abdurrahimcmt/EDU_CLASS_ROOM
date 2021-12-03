using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace EDU_Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Announcement> announcement { get; set; }
        //public IEnumerable<Category> Categories { get; set; }
    }
}
