using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.OracleProvider.OracleEntities;
namespace VR.Service.Services

{
    public interface IOraAppUserService
    {
        APPUSER Check(string username, string password);
        APPUSER CheckUserName(string username);
        int CheckRole(string username);
        APPUSER GetUserById(int Id);
        string GetFullNameByUserName(string userName);
        APPUSER GetUserByUserName(string userName);
    }
}
