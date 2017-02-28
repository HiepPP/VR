using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL;

namespace VR.Service.Services
{
    public interface IUserService
    {
        bool CheckVaild(string username, string password);

        int CheckRole(string username);
    }
}
