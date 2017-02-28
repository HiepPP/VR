using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL.Model;
using VR.Infrastructure.Utilities;

namespace VR.DAL.Repository
{
    public class ExcelDetailRepository : BaseRepository<ExcelDetail>, IExcelDetailRepository
    {
        // Get status            
        public TruckStatus? GetStatus(int excelDetailId)
        {
            Logger.Info("Get status");
            TruckStatus? status = new TruckStatus();

            try
            {
                using (var db = new VRContext())
                {
                    status = db.ExcelDetails.Where(x => x.Id == excelDetailId)
                               .Select(x => x.TruckStatus).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message, ex);
            }

            return status;
        }

        // Update status 
        public bool UpdateStatus(int excelDetailId, int newStatus)
        {
            Logger.Info("Update status");
            bool check = false;

            try
            {
                using (var db = new VRContext())
                {
                    var exDetail = db.ExcelDetails.SingleOrDefault(x => x.Id == excelDetailId);
                    if (exDetail != null)
                    {
                        exDetail.TruckStatus = (TruckStatus)newStatus;
                        db.SaveChanges();
                        check = true;
                    }
                    else
                        check = false;
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message, ex);
            }

            return check;
        }
    }
}
