using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.OracleProvider.OracleEntities;

namespace VR.Service.Services
{
    public interface IOraTruckService
    {
        TRUCK Insert(TRUCK truck);
        List<TRUCK> GetAll();
        TRUCK GetByTruckNo(string trkNo);
        TRUCK Update(TRUCK truck);
    }
}
