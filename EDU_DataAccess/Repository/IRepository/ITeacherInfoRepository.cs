using Microsoft.AspNetCore.Mvc.Rendering;
using EDU_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_DataAccess.Repository.IRepository
{
    public interface ITeacherInfoRepository : IRepository<TeacherInfo>
    {
        void Update(TeacherInfo obj);

        IEnumerable<SelectListItem> GetAllDropDownList(string obj);
    }
}
