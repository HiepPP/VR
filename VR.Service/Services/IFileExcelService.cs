using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VR.DAL.Model;

namespace VR.Service.Services
{
    public interface IFileExcelService
    {
        int CountTruckByExcelId(int Excel_Id);
        List<FileExcel> GetList();
        int[] Import(string voyCode, string userName, int partnerId = 0, HttpPostedFileBase fileImported = null, int Continue = 2);
        FileExcel GetById(int id);
        List<FileExcel> GetByVoyageNotDeparted();
    }
}
