using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL.Model;
using VR.OracleProvider.OracleEntities;

namespace VR.Service.Services
{
    public interface ICustomerService
    {
        PARTNER GetById(int id);

        List<PARTNER> GetListPartnerOracle(int voyId);

    }
}
