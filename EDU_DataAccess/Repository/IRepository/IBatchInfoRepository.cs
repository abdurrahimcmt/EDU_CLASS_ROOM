using Microsoft.AspNetCore.Mvc.Rendering;
using EDU_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_DataAccess.Repository.IRepository
{
    public interface IBatchinfoRepository : IRepository<InfoBatch>
    {
        void Update(InfoBatch obj);

        IEnumerable<SelectListItem> GetAllDropDownList(string obj);
    }
}
