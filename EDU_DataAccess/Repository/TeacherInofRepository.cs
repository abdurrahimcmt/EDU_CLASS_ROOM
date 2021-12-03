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
    public class TeacherInfoRepository : Repository<TeacherInfo>, ITeacherInfoRepository
    {
        private readonly ApplicationDbContext _db;
        public TeacherInfoRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if (obj==WC.Designation)
            {
                return _db.Designation.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            if (obj == WC.University)
            {
                return _db.UniversityName.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return null;
        }

        public void Update(TeacherInfo obj)
        {
            _db.TeacherInfo.Update(obj);
        }
    }
}
