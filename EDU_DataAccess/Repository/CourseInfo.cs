using Microsoft.AspNetCore.Mvc.Rendering;
using EDU_Utility;
using EDU_DataAccess.Data;
using EDU_DataAccess.Repository.IRepository;
using EDU_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_DataAccess.Repository
{
    public class CourseInfoRepository : Repository<CourseInfo>, ICourseInfoRepository
    {
        private readonly ApplicationDbContext _db;
        public CourseInfoRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj==WC.DepartmentName)
            {
                return _db.DepartmentInfo.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }

        public void Update(CourseInfo obj)
        {
            _db.CourseInfo.Update(obj);
        }

        
    }
}
