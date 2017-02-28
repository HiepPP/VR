using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL.Repository;
using VR.DAL.Model;
using VR.OracleProvider.OracleRepository;
using VR.OracleProvider.OracleEntities;
using VR.Infrastructure.Utilities;

namespace VR.Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IOraPartnerRepository _oraPartnerRepository = new OraPartnerRepository();
        private readonly IOraBillRepository _oraBillRepository = new OraBillRepository();
        private readonly IOraVoyageRepository _oraVoyageRepository = new OraVoyageRepository();
        public PARTNER GetById(int id)
        {
            return _oraPartnerRepository.GetById(id);
        }

        public List<PARTNER> GetListPartnerOracle(int voyId)
        {
            Logger.Info("GetListPartnerOracle voyId=" + voyId);
            try
            {           
                var lstBill = _oraBillRepository.FindBy(x => x.VOY_ID == voyId);
                var lstPartner = _oraPartnerRepository.GetAll();
                List<PARTNER> lstPartnerByVoyCode = lstPartner
                    .Where(x => lstBill.Any(y => y.PNR_ID == x.ID))
                    .ToList();
                return lstPartnerByVoyCode;
            }
            catch (Exception e)
            {
                Logger.Error("GetListPartnerOracle Error: ", e);
                throw e;
            }
        }
    }
}
