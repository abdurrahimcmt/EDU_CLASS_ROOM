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
    public class OnlineClassInofRepository : Repository<OnlineClassInfo>, IOnlineClassInfoRepository
    {
        private readonly ApplicationDbContext _db;
        public OnlineClassInofRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj == WC.CourseId)
            {
                return _db.CourseInfo.Select(i => new SelectListItem
                {
                    Text = i.Code+i.Name,
                    Value = i.Id.ToString()
                });
            }
            if (obj == WC.SemesterId)
            {
                return _db.SemesterInfo.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }

            if (obj == WC.DepartmentName)
            {
                return _db.DepartmentInfo.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }

            if (obj == WC.TeacherId)
            {
                return _db.TeacherInfo.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }
        public void Update(OnlineClassInfo obj)
        {
            _db.OnlineClassInfo.Update(obj);
        }
    }
}
