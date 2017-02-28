using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.OracleProvider.OracleRepository;
using VR.OracleProvider.OracleEntities;
namespace VR.Service.Services
{
    public class OraTruckService: IOraTruckService
    {
        private readonly IOraTruckRepository _truckrepo = new OraTruckRepository();
        public TRUCK Insert(TRUCK truck)
        {
            return _truckrepo.Insert(truck);
        }
        public List<TRUCK> GetAll()
        {
            return _truckrepo.GetAll().ToList();
        }
        public TRUCK GetByTruckNo(string trkNo)
        {
            return _truckrepo.SingleBy(x => x.TRK_NO.Equals(trkNo));
        }
        public TRUCK Update(TRUCK truck)
        {
            return _truckrepo.Update(truck);
        }
    }
}
