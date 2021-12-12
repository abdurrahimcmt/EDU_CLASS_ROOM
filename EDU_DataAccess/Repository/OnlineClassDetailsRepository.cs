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
    public class OnlineClassDetailsRepository : Repository<OnlineClassDetails>, IOnlineClassDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public OnlineClassDetailsRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(OnlineClassDetails obj)
        {
            _db.OnlineClassDetails.Update(obj);
        }
    }
}
