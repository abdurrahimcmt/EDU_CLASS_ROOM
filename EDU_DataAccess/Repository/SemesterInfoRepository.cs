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
    public class SemesterInfoRepository : Repository<SemesterInfo>, ISemesterInfoRepository
    {
        private readonly ApplicationDbContext _db;
        public SemesterInfoRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(SemesterInfo obj)
        {
            var objFromDb = base.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb!=null)
            {
                objFromDb.Name = obj.Name;
            }
        }
    }
}
