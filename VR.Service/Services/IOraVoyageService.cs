using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.OracleProvider.OracleEntities;

namespace VR.Service.Services
{
    public interface IOraVoyageService
    {
        VOYAGE Insert(VOYAGE truck);
        List<VOYAGE> GetAll();
        List<VOYAGE> GetVoyATD();
        VOYAGE GetByVoyCode(string voyCode);

        VOYAGE GetById(int id);
    }
}
