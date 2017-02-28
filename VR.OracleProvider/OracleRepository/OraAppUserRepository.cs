using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.OracleProvider.OracleEntities;

namespace VR.OracleProvider.OracleRepository
{
    public class OraAppUserRepository : OraBaseRepository<APPUSER>, IOraAppUserRepository
    {
        public int CheckRole(string username)
        {
            using (OracleContext ctx = new OracleContext())
            {
                //query checkrole return value in ORACLE
                int res = ctx.Database.SqlQuery<int>($"select SYSTEM.CHECKROLE ('{username}') from dual").FirstOrDefault();
                return res;
            }
        }
    }
}