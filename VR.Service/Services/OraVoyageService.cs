using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.Infrastructure.Utilities;
using VR.OracleProvider.OracleEntities;
using VR.OracleProvider.OracleRepository;

namespace VR.Service.Services
{
    public class OraVoyageService : IOraVoyageService
    {
        private readonly IOraVoyageRepository _oraVoyageRepository = new OraVoyageRepository();
        public VOYAGE Insert(VOYAGE voyage)
        {
            return _oraVoyageRepository.Insert(voyage);
        }
        public List<VOYAGE> GetAll()
        {
            return _oraVoyageRepository.GetAll().ToList();
        }

        public List<VOYAGE> GetVoyATD()
        {
            Logger.Info("GetVoyATD");
            try
            {
               //Filter voyages which has ATD null
                List<VOYAGE> lstVoyage = _oraVoyageRepository.FindBy(x => x.ATD == null).ToList();
                if (lstVoyage.Count() > 0)
                {
                    return lstVoyage;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Logger.Error("GetVoyATD Error: ", e);
                throw e;
            }
        }

        public VOYAGE GetByVoyCode(string voyCode)
        {
            Logger.Info("GetVoyATD");
            try
            {
                //Find voyage has CODE is voyCode
                VOYAGE voyage = _oraVoyageRepository.SingleBy(x => x.CODE.Equals(voyCode));
                if (voyage != null)
                {
                    return voyage;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Logger.Error("GetVoyATD Error: ", e);
                throw e;
            }
        }

        public VOYAGE GetById(int id)
        {
            return _oraVoyageRepository.SingleBy(x => x.ID == id);
        }
    }
}
