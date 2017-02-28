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
using VR.Service.Services;

namespace VR.Service.Services
{
    /// <summary>
    /// Class include functions which handle assigning Truck to Voyage
    /// </summary>
    public class AssignService : IAssignService
    {
        IOraTruckService _oraTruckService;
        IOraTruckPartnerRepository _oraTruckPartnerRepository;
        IOraVoyageService _oraVoyageService;
        IOraPartnerRepository _oraPartnerRepository;
        ///
        /// <summary>
        /// Assign Truck to Voyage
        /// </summary>
        /// <param name="trkNo">Truck No</param>
        /// <param name="voyId">Voyage Id</param>
        /// <param name="partnerId">Partner id</param>
        /// <returns></returns>
        /// 
        public bool AssignTrkToVoy(string trkNo, string voyCode, int partnerId)
        {
            Logger.Info("AssignTrkToVoy trkNo=" + trkNo + " voyCode=" + voyCode + " partnerId=" + partnerId);
            try
            {
                PARTNER_TRUCK assigned;
                _oraTruckService = new OraTruckService();
                _oraTruckPartnerRepository = new OraTruckPartnerRepository();
                _oraVoyageService = new OraVoyageService();
                _oraPartnerRepository = new OraPartnerRepository();
                //Find Voyage by voyage code
                var voyage = _oraVoyageService.GetByVoyCode(voyCode);
                //Find Truck by truck no
                var truck = _oraTruckService.GetByTruckNo(trkNo);
                //Find Partner by Id
                var partner = _oraPartnerRepository.GetById(partnerId);
                //Check if voyage or truck or parner is in database
                if (voyage == null || truck == null || partner == null)
                {
                    return false;
                }
                //Create an Assigned object
                assigned = new PARTNER_TRUCK
                {
                    PNR_ID = partnerId,
                    TRK_ID = truck.ID,
                    VOY_ID = voyage.ID
                };
                _oraTruckPartnerRepository.Insert(assigned);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error("AssignTrkToVoy Error: ", e);
                throw e;
            }
        }
    }
}


