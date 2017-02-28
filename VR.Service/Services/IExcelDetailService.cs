using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL.Model;

namespace VR.Service.Services
{
    public interface IExcelDetailService
    {
        ExcelDetail GetById(int id);

        int GetStatus(int id);

        ExcelDetail Insert(ExcelDetail fileDetail);

        List<ExcelDetail> GetListByFileExcelId(int fileExcelId);

        List<ExcelDetail> GetListByStatus(int fileExcelId, int truckStatus);
        ExcelDetail SingleBy(string TruckNo);
        ExcelDetail Update(ExcelDetail exceldetail);

    }
}
