using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.OracleProvider.OracleEntities;
using VR.OracleProvider.OracleRepository;

namespace VR.Service.Services
{
    public class OraTruckPartnerRepository:OraBaseRepository<PARTNER_TRUCK>, IOraTruckPartnerRepository
    {
    }
}
