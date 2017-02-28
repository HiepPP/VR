using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL.Model;
using VR.OracleProvider.OracleEntities;

namespace VR.Service.Services
{
    public interface IVoyageSerivce
    {
        VOYAGE GetById(int id);

        List<VOYAGE> GetVoyATD();
    }
}
