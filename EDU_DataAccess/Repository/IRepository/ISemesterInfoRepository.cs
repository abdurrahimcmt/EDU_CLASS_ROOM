﻿using EDU_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDU_DataAccess.Repository.IRepository
{
    public interface ISemesterInfoRepository : IRepository<SemesterInfo>
    {
        void Update(SemesterInfo obj);
    }
}
