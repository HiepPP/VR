using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.OracleProvider.OracleRepository;
using VR.OracleProvider.OracleEntities;

namespace VR.Service.Services
{
        public class OraAppUserService : IOraAppUserService
        {
            //private readonly IOraAppUserRepository _oraAppUserRepo = new OraAppUserRepository();
            IOraAppUserRepository _oraAppUser = new OraAppUserRepository();
            public APPUSER Check(string username, string password)
            {
                var r = _oraAppUser.SingleBy(x => x.USER_NAME == username && x.PWD == password);
                return r;
            }
            public APPUSER CheckUserName(string username)
            {
                return _oraAppUser.SingleBy(x => x.USER_NAME == username);
            }
            public int CheckRole(string username)
            {
                return _oraAppUser.CheckRole(username);
            }
            //Get AppUser By ID
            public APPUSER GetUserById(int Id)
            {
                return _oraAppUser.SingleBy(x => x.ID == Id);
            }

        public string GetFullNameByUserName(string userName)
        {
            return _oraAppUser.SingleBy(x => x.USER_NAME.Equals(userName)).FULL_NAME;
        }

        public APPUSER GetUserByUserName(string userName)
        {
            return _oraAppUser.SingleBy(x => x.USER_NAME.Equals(userName));
        }
    }
    }
