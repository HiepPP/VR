using System.Collections.Generic;
using System.Web.Mvc;
using VR.Service.Services;
using VR.Web.UI.ViewModel;
using VR.Infrastructure.Utilities;
using VR.DAL.Repository;
using System;
using VR.DAL.Model;
using VR.Web.UI.Language;
using System.Data.Entity;
using VR.OracleProvider.OracleEntities;
using System.Text.RegularExpressions;
using System.Linq;

namespace VR.Web.UI.Controllers
{
    /// <summary>
    /// Display ViewRegitrasionInformation or LogInfomation
    /// </summary>
    [Authorize]
    public class LogController : Controller
    {
        private readonly IExcelDetailService _excelDetailService = new ExcelDetailService();
        private readonly IFileExcelService _fileExeclService = new FileExcelService();
        private readonly ICustomerService _cusService = new CustomerService();
        private readonly IOraAppUserService _oraAppUserService = new OraAppUserService();
        private readonly IOraVoyageService _oraVoyage = new OraVoyageService();

        public ActionResult Index()
        {
            return View();
        }

        // Display ViewRegitrasionInformation
        public ActionResult ViewRegitrasionInfomation(int? fileExcelId, int? truckStatus)
        {
            Logger.Info("ViewRegitrasionInfomation for file Excel with id = " + fileExcelId);
            List<ExcelDetail> list = null;
            ListResultViewModel listModel = null;
            // Filter by truckStatus if it have. If not will get all
            try
            {
                ViewBag.fileExcelId = fileExcelId ?? default(int);
                list = new List<ExcelDetail>();
                // Get list of excel rows was filtered
                if (truckStatus.HasValue)
                {
                    list = _excelDetailService.GetListByStatus(fileExcelId ?? default(int), truckStatus.Value);
                }
                // Get all excel rows
                else
                {
                    list = _excelDetailService.GetListByFileExcelId(fileExcelId ?? default(int));
                }
                var fileExcel = _fileExeclService.GetById(fileExcelId ?? default(int));
                var listExcelDetail = new List<ResultViewModel>();
                // Assign data to list ResultViewModel
                foreach (var item in list)
                {
                    string result = DefineStatus(item.TruckStatus);
                    var rowResult = new ResultViewModel
                    {
                        TruckNo = item.TruckNo,
                        ChassicNo = item.ChassisNo,
                        DriverName = item.DriverName,
                        DriverId = item.DriverId,
                        Result = result,
                        TruckStatus = item.TruckStatus
                    };
                    listExcelDetail.Add(rowResult);
                }
                // Assign data to ListReultViewModel
                listModel = new ListResultViewModel();
                listModel.VesselCode = _oraVoyage.GetByVoyCode(fileExcel.VoyCode).CODE;
                listModel.VesselName = _oraVoyage.GetByVoyCode(fileExcel.VoyCode).NAME;
                listModel.Customer = _cusService.GetById(fileExcel.CustomerId ?? default(int)).NAME;
                listModel.FileName = fileExcel.ExcelName;
                listModel.ResultViewModels = listExcelDetail;
                // Create ListItem for FliterDropdownlist 
                List<SelectListItem> listItem = FilterDropDown(truckStatus);
                ViewBag.Items = listItem;
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message, ex);
                return RedirectToAction("Error", "Log");
            }
            return View("ViewRegitrasionInfomation", listModel);
        }

        // Create ListItem for FliterDropdownlist 
        public static List<SelectListItem> FilterDropDown(int? truckStatus)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem
            {
                Text = Resource_ExcelDetail.All,
                Value = null,
                Selected = CheckValue(truckStatus, null)
            });
            listItem.Add(new SelectListItem
            {
                Text = Resource_ExcelDetail.NotYetCheck,
                Value = (int)TruckStatus.NotYetCheck + "",
                Selected = CheckValue(truckStatus, (int)TruckStatus.NotYetCheck)

            });
            listItem.Add(new SelectListItem
            {
                Text = Resource_ExcelDetail.GetInfoAndValid,
                Value = (int)TruckStatus.GetInfoAndValid + "",
                Selected = CheckValue(truckStatus, (int)TruckStatus.GetInfoAndValid)
            });
            listItem.Add(new SelectListItem
            {
                Text = Resource_ExcelDetail.GetInfoButNotValid,
                Value = (int)TruckStatus.GetInfoButNotValid + "",
                Selected = CheckValue(truckStatus, (int)TruckStatus.GetInfoButNotValid)
            });
            listItem.Add(new SelectListItem
            {
                Text = Resource_ExcelDetail.ErrorWhileChecking,
                Value = (int)TruckStatus.ErrorWhileChecking + "",
                Selected = CheckValue(truckStatus, (int)TruckStatus.ErrorWhileChecking)
            });
            listItem.Add(new SelectListItem
            {
                Text = Resource_ExcelDetail.HaveInfoAndValid,
                Value = (int)TruckStatus.HaveInfoAndValid + "",
                Selected = CheckValue(truckStatus, (int)TruckStatus.HaveInfoAndValid)
            });
            return listItem;
        }
        List<SelectListItem> listItem = new List<SelectListItem>();

        // Check two values are equal 
        public static bool CheckValue(int? value1, int? value2)
        {
            try
            {
                if (value1 == value2)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message, ex);
            }
            return false;
        }

        // Define Status Result by TruckStatus
        public static string DefineStatus(TruckStatus status)
        {
            string result;
            try
            {
                // Assign data from Resource_ExcelDetail for exceldetail result
                switch (status)
                {
                    case TruckStatus.NotYetCheck:
                        return result = Resource_ExcelDetail.NotYetCheck;
                    case TruckStatus.GetInfoAndValid:
                        return result = Resource_ExcelDetail.GetInfoAndValid;
                    case TruckStatus.GetInfoButNotValid:
                        return result = Resource_ExcelDetail.GetInfoButNotValid;
                    case TruckStatus.HaveInfoAndValid:
                        return result = Resource_ExcelDetail.HaveInfoAndValid;
                    case TruckStatus.ErrorWhileChecking:
                        return result = Resource_ExcelDetail.ErrorWhileChecking;
                    default:
                        return result = "";
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message, ex);
            }
            return result = "";
        }

        // Display LogInformation
        public ActionResult LogInformation(int? filter)
        {
            Logger.Info("LogInformation");
            // Create ListItem for FliterDropdownlist 
            List<SelectListItem> listItem = LogFilterDropDown(filter);
            ViewBag.LogFilter = listItem;
            List<LogViewModel> logViewModel = new List<LogViewModel>();
            List<FileExcel> listFileExcel = new List<FileExcel>();
            try
            {
                if (filter == (int)LogFilter.NotDeparted)
                {
                    // Get all file excel was filter by voyage 
                    listFileExcel = _fileExeclService.GetByVoyageNotDeparted();
                }
                else
                {
                    // Get all file excel had imported
                    listFileExcel = _fileExeclService.GetList();
                }
                int countImport = 0;
                int countUpdateStatus = 0;
                int totalDetail = 0;
                // Add each file excel detail to LogViewModel
                foreach (var item in listFileExcel)
                {
                    var logRow = new LogViewModel();
                    // Count how many excel detail had imported
                    countImport = _excelDetailService.GetListByFileExcelId(item.Id)
                                  .Where(x => x.LinkedId == null).ToList().Count;
                    // Count how many excel detail had update status
                    countUpdateStatus = _excelDetailService.GetListByFileExcelId(item.Id)
                                        .Where(x => x.TruckStatus != TruckStatus.NotYetCheck).Count();
                    // Count total excel row
                    totalDetail = _excelDetailService.GetListByFileExcelId(item.Id).Count;
                    logRow.ImpDate = item.Created;
                    logRow.VesselCode = _oraVoyage.GetByVoyCode(item.VoyCode).CODE;
                    logRow.CustomerName = _cusService.GetById(item.CustomerId ?? default(int)).NAME;
                    logRow.ImpUser = _oraAppUserService.GetFullNameByUserName(item.UserName);
                    logRow.Continue = "<a href=\"/Checking/GetInfoRegis?excelId="
                                      + item.Id + "\">Continue</a>";
                    logRow.View = "<a href=\"/Log/ViewRegitrasionInfomation?fileExcelId="
                                  + item.Id + "\">View</a>";
                    logRow.FileName = item.ExcelName;
                    logRow.RowImported = countImport + "/" + item.RowTotal + "log";
                    logRow.IsDone = item.IsDone;
                    logRow.ExcelId = item.Id;
                    // Check all exceldetail had updated status
                    if (CheckValue(countUpdateStatus, totalDetail))
                    {
                        logRow.Status = "Done(" + countUpdateStatus + "/" + totalDetail + ")";
                        logRow.IsUpdate = true;
                    }
                    else
                    {
                        logRow.Status = "Not Finished(" + countUpdateStatus + "/" + totalDetail + ")";
                        logRow.IsUpdate = false;
                    }
                    // Add each log row to LogViewModel
                    logViewModel.Add(logRow);
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message, ex);
            }
            return View(logViewModel);
        }

        // Create filter dropdownlist by voyage
        public static List<SelectListItem> LogFilterDropDown(int? filter)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem
            {
                Text = Resource_Log.All,
                Value = (int)LogFilter.All + "",
                Selected = CheckValue(filter, (int)LogFilter.All)
            });
            listItem.Add(new SelectListItem
            {
                Text = Resource_Log.Departed,
                Value = (int)LogFilter.NotDeparted + "",
                Selected = CheckValue(filter, (int)LogFilter.NotDeparted)
            });
            return listItem;
        }

        // Display Error
        public ViewResult Error()
        {
            return View();
        }

        // Download file excel 
        public ActionResult DownLoad(int fileExcelId, string fileExcelName)
        {
            Logger.Info("Download file excel: " + fileExcelName);
            string filePath = "";
            string type = "application/vnd.ms-excel"
                ;
            try
            {
                // Get disk contain file excel
                string value = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"];
                var dayCreated = _fileExeclService.GetById(fileExcelId).Created.ToString("dd/MM/yyyy");
                // Get folder contain file excel
                string folder = FileExcelService.RemoveSymbol(dayCreated);
                // Create path
                filePath = value + "\\" + folder + "\\" + fileExcelName;
                if (!System.IO.File.Exists(filePath))
                {
                    return RedirectToAction("ViewRegitrasionInfomation", "Log", new { fileExcelId = fileExcelId });
                }
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message + "path:" + filePath, ex);
            }
            return File(filePath, type, fileExcelName);
        }

        public enum LogFilter
        {
            NotDeparted = 1,
            All = 2,
        }

    }
}