using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL.Repository;
using VR.DAL.Model;
using VR.Infrastructure.Utilities;
using VR.OracleProvider.OracleRepository;
using VR.OracleProvider.OracleEntities;

namespace VR.Service.Services
{
    public class VoyageSerivce : IVoyageSerivce
    {
        IOraVoyageRepository _oraVoyageRepository;
        public VOYAGE GetById(int id)
        {
            _oraVoyageRepository = new OraVoyageRepository();
            return _oraVoyageRepository.GetById(id);
        }
        ///
        /// <summary>
        /// Lấy danh sách tàu chưa rời cảng
        /// </summary>
        /// <returns></returns>
        /// 
        public List<VOYAGE> GetVoyATD()
        {
            Logger.Info("GetVoyATD");
            try
            {
                _oraVoyageRepository = new OraVoyageRepository();
                //Lọc các tàu chưa rời cảng có ATD là null
                List<VOYAGE> lstVoyage = _oraVoyageRepository.FindBy(x => x.ATD == null).ToList();
                //Trả về danh sách chỉ khi có giá trị
                if (lstVoyage.Count() > 0)
                {
                    return lstVoyage;
                }
                else
                {
                    return null;
                }
            }catch(Exception e)
            {
                Logger.Error("GetVoyATD Error: ", e);
                throw e;
            }
        }
    }
}
