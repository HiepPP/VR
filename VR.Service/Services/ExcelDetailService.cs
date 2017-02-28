using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL.Repository;
using VR.DAL.Model;

namespace VR.Service.Services
{
    public class ExcelDetailService : IExcelDetailService
    {

        private readonly IExcelDetailRepository _excelDetailRepository = new ExcelDetailRepository();

        public ExcelDetail GetById(int id)
        {
            return _excelDetailRepository.GetById(id);
        }

        // Get all excel row in excel file
        public List<ExcelDetail> GetListByFileExcelId(int fileExcelId)
        {
            return _excelDetailRepository.FindBy(x => x.FileExcelId == fileExcelId).ToList();
        }

        // Get all excel row in excel file by status
        public List<ExcelDetail> GetListByStatus(int fileExcelId, int truckStatus)
        {
            return _excelDetailRepository.FindBy(x => x.FileExcelId == fileExcelId 
                                        && (int)x.TruckStatus == truckStatus).ToList();
        }

        public int GetStatus(int id)
        {
            throw new NotImplementedException();
        }

        public ExcelDetail Insert(ExcelDetail fileDetail)
        {
            throw new NotImplementedException();
        }
        public ExcelDetail SingleBy(string TruckNo)
        {
            return _excelDetailRepository.SingleBy(x => x.TruckNo == TruckNo);
        }
        //public IEnumerable<ExcelDetail> GetListByFileExcelId(int fileExcelId)
        //{
        //    return _exRepository.FindBy(x => x.FileExcelId == fileExcelId);
        //}
        public ExcelDetail Update(ExcelDetail exceldetail)
        {
            return _excelDetailRepository.Update(exceldetail);
        }
    }
}
