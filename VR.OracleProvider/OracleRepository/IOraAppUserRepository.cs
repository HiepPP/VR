﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.OracleProvider.OracleEntities;

namespace VR.OracleProvider.OracleRepository
{
    public interface IOraAppUserRepository : IOraBaseRepository<APPUSER>
    {
        int CheckRole(string username);
    }
}