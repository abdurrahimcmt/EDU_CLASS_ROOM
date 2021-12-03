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
    public class EnrollmentDetailsRepository : Repository<EnrollmentDetails>, IEnrollmentDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public EnrollmentDetailsRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        public void Update(EnrollmentDetails obj)
        {
            _db.EnrollmentDetails.Update(obj);
        }
    }
}
