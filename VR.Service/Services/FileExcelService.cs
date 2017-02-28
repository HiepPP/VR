using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VR.DAL.Repository;
using VR.DAL.Model;
using System.Web;
using System.IO;
using System.Data;
using VR.Infrastructure.Utilities;
using System.Web.Configuration;
using System.Threading;
using System.Text.RegularExpressions;
using VR.OracleProvider.OracleEntities;
using OfficeOpenXml;
using VR.OracleProvider.OracleRepository;

namespace VR.Service.Services
{
    /// <summary>
    /// Class used for uploading and save file process
    /// </summary>
    public class FileExcelService : IFileExcelService
    {
        private readonly IExcelRepository _fileExcelRepository;
        private readonly IExcelDetailRepository _excelDetailRepository;
        private readonly IOraTruckRepository _oraTruckRepository;
        private readonly IAssignService _assignService;
        private readonly IOraVoyageService _oraVoyageService;
        private readonly IOraVoyageRepository _oraVoyageRepository;
        private readonly IOraTruckService _oraTruckService;
        private const string STR_CONST_TRUCK = "T";
        private const string STR_CONST_TRACTOR = "V";
        private const string STR_CONST_REMOOC = "R";
        public FileWrapper fw;
        //Initialize;
        public FileExcelService()
        {
            fw = new FileWrapper();
            _fileExcelRepository = new ExcelRepository();
            _excelDetailRepository = new ExcelDetailRepository();
            _oraTruckRepository = new OraTruckRepository();
            _assignService = new AssignService();
            _oraVoyageService = new OraVoyageService();
            _oraVoyageRepository = new OraVoyageRepository();
            _oraTruckService = new OraTruckService();
        }
        public FileExcelService(FileWrapper fw)
        {
            this.fw = fw;
            _fileExcelRepository = new ExcelRepository();
            _excelDetailRepository = new ExcelDetailRepository();
            _oraTruckRepository = new OraTruckRepository();
            _assignService = new AssignService();
            _oraVoyageService = new OraVoyageService();
            _oraVoyageRepository = new OraVoyageRepository();
            _oraTruckService = new OraTruckService();
        }
        //Get FileWrapper object    
        public FileWrapper CheckExisted()
        {
            return fw;
        }
        ///
        /// <summary>
        /// Count Trucks stored in database by using Id of File Excel imported before
        /// </summary>
        /// <param name="excelId">Id file excel</param>
        /// <returns></returns>
        /// 
        public int CountTruckByExcelId(int excelId)
        {
            throw new NotImplementedException();
        }
        ///
        /// <summary>
        /// Get List of Imported File Excel 
        /// </summary>
        /// <returns>List<FileExcel></returns>
        /// 
        //Get List File Excel
        public List<FileExcel> GetList()
        {
            return _fileExcelRepository.GetAll().ToList();
        }
        ///
        /// <summary>
        /// Update File Excel's status stored in database
        /// </summary>
        /// <param name="file">File need to update its status</param>
        /// <param name="status">status which file need to change into</param>
        /// <returns></returns>
        /// 
        public bool UpdateStatusExcelFile(FileExcel file, bool status = false)
        {
            Logger.Info("UpdateStatusExcelFile status=" + status);
            try
            {
                file.IsDone = status;
                var entity = _fileExcelRepository.Update(file);
                if (entity == null)
                    return false;
                else return true;
            }
            catch (Exception e)
            {
                Logger.Error("UpdateStatusExcelFile Error: ", e);
                throw e;
            }
        }
        ///
        /// <summary>
        /// Store file excel uploaded and its content in to computer and database server
        /// Execute function AssignTrkToVoy behind the scene
        /// </summary>
        /// <param name="voyCode">Voyage Code</param>
        /// <param name="userName">Id of user who use this function (default 0 if there is no value)</param>
        /// <param name="partnerId">If of customer (default 0 if there is no value)</param>
        /// <param name="fileImported">file which will be handled(null if there is no file)</param>
        /// <param name="Continue">Continue to import file from row Continue</param>
        /// <returns></returns>
        /// 
        public int[] Import(string voyCode, string userName, int partnerId = 0, HttpPostedFileBase fileImported = null, int Continue = 2)
        {
            Logger.Info("Import voyCode=" + voyCode + " userId=" + userName + " customerId=" + partnerId + " fileImported=" + fileImported.FileName);
            int[] result = new int[2];
            try
            {
                //Check if any input is null
                if (string.IsNullOrEmpty(voyCode) || string.IsNullOrEmpty(userName) || partnerId == 0 || fileImported == null)
                {
                    result[0] = (int)ImportedFileState.NotEnoughParams; //Not enough params
                    return result;
                }
                //Check if file's content is not blank
                if (fileImported.ContentLength > 0)
                {
                    //get ToDay's Date
                    string toDay = DateTime.Now.ToString("dd/MM/yyyy");
                    toDay = RemoveSymbol(toDay);
                    //get directory in web config used for saving file
                    string uploadDirectory = WebConfigurationManager.AppSettings["UploadDirectory"];
                    //set path which file will be saved into
                    string path = string.Format("{0}/{1}/{2}", uploadDirectory, toDay, fileImported.FileName);
                    //Check if file's directory not exist
                    if (!Directory.Exists(path))
                    {
                        //Create directory used for saving file
                        string directory = string.Format("{0}/{1}", uploadDirectory, toDay);
                        Directory.CreateDirectory(directory);
                    }
                    //Check if file is in the right extension
                    if (IsExcel(fileImported))
                    {
                        int[] resultSaveAndReadFile = SaveAndReadFile(path, fileImported, voyCode, partnerId, userName, Continue);
                        if (resultSaveAndReadFile[0] != (int)ImportedFileState.Done)
                        {
                            result[0] = resultSaveAndReadFile[0];
                            return result;
                        }
                        else
                        {
                            result = resultSaveAndReadFile;
                            return result;
                        }
                    }
                    else
                    {
                        result[0] = (int)ImportedFileState.WrongExtension;
                        return result; //Wrong Extension
                    }
                }
                else
                {
                    result[0] = (int)ImportedFileState.ContentBlank; //The content is blank
                    return result;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Import Error: ", e);
                throw e;
            }
        }
        /// <summary>
        /// Save and read file Excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileImported"></param>
        /// <param name="voyCode"></param>
        /// <param name="partnerId"></param>
        /// <param name="userName"></param>
        /// <param name="Continue"></param>
        /// <returns></returns>
        private int[] SaveAndReadFile(string path, HttpPostedFileBase fileImported, string voyCode, int partnerId, string userName, int Continue)
        {
            
            int[] result = new int[2];
            //Check if file name already existed
            if (fw.Exist == true || fw.Exists(path))
            {
                result[0] = (int)ImportedFileState.Existed;//Existed
                return result;
            }
            //Save file to computer and database
            FileExcel fileExcel = SaveExcel(fileImported, voyCode, path, partnerId, userName);
            //Read Excel File's content
            List<ExcelDetail> lstExcelDetail = ReadFile(fileImported, fileExcel.Id, Continue);
            if (lstExcelDetail == null)
            {
                result[0] = (int)ImportedFileState.ContentSizeWrong;//Content size wrong
                return result;
            }
            fileExcel.RowTotal = lstExcelDetail.Count;
            _fileExcelRepository.Update(fileExcel);
            //Check and execute saving file's content
            if (!SaveExcelDetail(lstExcelDetail, voyCode, partnerId))
            {
                result[0] = (int)ImportedFileState.NotFinished;//Not finished
                return result;
            }
            else
            {
                fileExcel.IsDone = true; //Already saved file's content
                _fileExcelRepository.Update(fileExcel);
                result[0] = (int)ImportedFileState.Done;//Not finished
                result[1] = fileExcel.Id;
                return result;
            }
        }

        /// <summary>
        /// Save Excel on computer and info on database
        /// </summary>
        /// <param name="fileImported">file need to be saved</param>
        /// <param name="voyCode">Voyage Code</param>
        /// <param name="path">path need to be saved to</param>
        /// <param name="customerId">Customer Id</param>
        /// <param name="userId">User Id</param>
        /// <returns></returns>
        private FileExcel SaveExcel(HttpPostedFileBase fileImported, string voyCode, string path, int customerId, string userName)
        {
            Logger.Info("SaveExcel voyCode=" + voyCode
                + " path=" + path
                + " customerId=" + customerId
                + " userName=" + userName);
            try
            {
                //Initialize FileExcel object
                FileExcel fileExcel = new FileExcel
                {
                    DateImport = DateTime.Now,
                    ExcelName = fileImported.FileName,
                    IsDone = false,
                    CustomerId = customerId,
                    VoyCode = voyCode,
                    UserName = userName
                };
                //store to computer
                fileImported.SaveAs(path);
                fw.Exist = true;
                //Add FileExcel object to database
                fileExcel = _fileExcelRepository.Insert(fileExcel);
                return fileExcel;
            }
            catch (Exception e)
            {
                Logger.Error("SaveExcel Error: ", e);
                throw e;
            }
        }
        ///
        /// <summary>
        /// Check if the file is Excel
        /// </summary>
        /// <param name="file">file need to be checked</param>
        /// <returns></returns>
        /// 
        private bool IsExcel(HttpPostedFileBase file)
        {
            Logger.Info("IsExcel FileName=" + file.FileName);
            try
            {
                string extension = Path.GetExtension(file.FileName).ToLower();
                string[] validFileTypes = { ".xls", ".xlsx" };
                //Check if file's extension is valid
                if (validFileTypes.Contains(extension))
                {
                    return true;
                }
                else return false;
            }
            catch (Exception e)
            {
                Logger.Error("IsExcel Error: ", e);
                throw e;
            }
        }
        ///
        /// <summary>
        /// Save file's content to database
        /// Separate Truck and Remooc
        /// </summary>
        /// <param name="lstExcelDetail">the content need to be saved</param>
        /// <param name="voyId">Voyage Id</param>
        /// <returns></returns>
        /// 
        private bool SaveExcelDetail(List<ExcelDetail> lstExcelDetail, string voyCode, int partnerId)
        {
            Logger.Info("SaveExcelDetail voyCode=" + voyCode);
            try
            {
                foreach (var detail in lstExcelDetail)
                {
                    //Check if truck in file content has TruckNo
                    if (!string.IsNullOrEmpty(detail.TruckNo))
                    {
                        VOYAGE voyage = _oraVoyageService.GetByVoyCode(voyCode);
                        //Get Truck's status
                        detail.TruckStatus = IsTruckValidForVoy(detail.TruckNo, voyage.ID);
                        ExcelDetail entityTruck = null;
                        ExcelDetail entityRemooc = null;
                        ExcelDetail detailRemooc = null;
                        //Check if truck has remooc
                        if (!string.IsNullOrEmpty(detail.ChassisNo))
                        {
                            //Create ExcelDetail object contains info of remooc
                            detailRemooc = SetRemoocInfo(detail, voyage.ID);
                            //set Truck's remooc null
                            detail.ChassisNo = null;
                        }
                        //entity ExcelDetail contains Truck's info
                        entityTruck = _excelDetailRepository.Insert(detail);
                        //Check if there is remooc
                        if (detailRemooc != null)
                        {
                            detailRemooc.LinkedId = entityTruck.Id; //Remooc used by tractor                           
                            entityRemooc = _excelDetailRepository.Insert(detailRemooc);
                        }
                        //Not null if inserting ExcelDetail object successfully
                        if (entityTruck != null)
                        {
                            //Check if entity ExcelDetail contains Remooc's info is not null
                            if (entityRemooc != null)
                            {
                                //Contractor has remooc
                                InsertTruckAfterChecking(entityTruck, STR_CONST_TRACTOR);
                                InsertTruckAfterChecking(entityRemooc, STR_CONST_REMOOC);
                                AssignTruckToVoyage(entityTruck.TruckNo, voyCode, partnerId);
                                AssignTruckToVoyage(entityRemooc.TruckNo, voyCode, partnerId);
                            }
                            else
                            {
                                //Truck Type
                                InsertTruckAfterChecking(entityTruck, STR_CONST_TRUCK);
                                AssignTruckToVoyage(entityTruck.TruckNo, voyCode, partnerId);
                            }
                        }
                        else
                        {
                            return false; //Can't add
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("SaveExcelDetail Error: ", e);
                throw e;
            }
            return true;
        }
        /// <summary>
        /// Execute assign truck to voyage behind the scene
        /// </summary>
        /// <param name="truckNo"></param>
        /// <param name="voyCode"></param>
        /// <param name="partnerId"></param>
        private void AssignTruckToVoyage(string truckNo, string voyCode, int partnerId)
        {
            (new Thread(() =>
                                _assignService.AssignTrkToVoy(truckNo, voyCode, partnerId)
                          )).Start();
        }

        ///
        /// <summary>
        /// Store truck's info in file excel into database
        /// </summary>
        /// <param name="detail">truck's info</param>
        /// 
        private void InsertTruckAfterChecking(ExcelDetail detail, string truckType)
        {
            Logger.Info("InsertTruck truckType=" + truckType);
            try
            {
                TRUCK truck = null;
                if (!string.IsNullOrEmpty(detail.TruckNo))
                {
                    //Find truck in database by Truck No
                    truck = _oraTruckService.GetByTruckNo(detail.TruckNo);
                }
                else
                {
                    //Not a truck or tractor
                    //Find truck by Remooc No
                    truck = _oraTruckService.GetByTruckNo(detail.ChassisNo);
                }
                if (truck == null)
                {
                    //There isn't the truck in database before
                    //Create new truck object
                    List<ExcelDetail> lstExcel = new List<ExcelDetail>();
                    lstExcel.Add(detail);
                    //get Truck No or Chassis No
                    var TruckNo = !string.IsNullOrEmpty(detail.TruckNo) ? detail.TruckNo : detail.ChassisNo;
                    TRUCK newTruck = new TRUCK
                    {

                        TRK_NO = TruckNo,
                        TRK_TYPE = truckType
                    };
                    var addedTruck = _oraTruckRepository.Insert(newTruck);
                }
            }
            catch (Exception e)
            {
                Logger.Error("InsertTruck Error: ", e);
                throw e;
            }
        }

        ///
        /// <summary>
        /// Create Remooc object from row data of file excel
        /// </summary>
        /// <param name="detail">Row data of file excel read before</param>
        /// <param name="voyId">Voyage Id</param>
        /// <returns></returns>
        /// 
        private ExcelDetail SetRemoocInfo(ExcelDetail detail, int voyId)
        {
            Logger.Info("SetChassisInfo voyId=" + voyId);
            try
            {
                //Set value for Remooc object
                ExcelDetail RemoocInfo = new ExcelDetail
                {
                    ChassisNo = detail.ChassisNo,
                    LinkedId = detail.Id,
                    DriverId = detail.DriverId,
                    DriverName = detail.DriverName,
                    LicenseNo = detail.LicenseNo,
                    FileExcelId = detail.FileExcelId
                };
                //Get Remooc's status
                RemoocInfo.TruckStatus = IsTruckValidForVoy(RemoocInfo.ChassisNo, voyId);
                return RemoocInfo;
            }
            catch (Exception e)
            {
                Logger.Error("SetChassisInfo Error", e);
                throw e;
            }
        }
        ///
        /// <summary>
        /// Check if Truck or Remooc's checking date still valid
        /// </summary>
        /// <param name="trkNo">Truck(or Remooc) No</param>
        /// <param name="voyId">Voyage Id</param>
        /// <returns></returns>
        /// 
        private TruckStatus IsTruckValidForVoy(string trkNo, int voyId)
        {
            Logger.Info("SaveExcelDetail voyId=" + voyId);
            try
            {
                //Get a Truck by Truck No
                TRUCK truck = _oraTruckService.GetByTruckNo(trkNo);
                //Get a Voyage by Voyage Id
                VOYAGE voyage = _oraVoyageRepository.GetById(voyId);
                if (truck != null)
                {
                    if (truck.GCN_DATE.HasValue)
                    {
                        //Check if voyage has ATB time
                        if (voyage.ATB.HasValue)
                        {
                            //Check if Truck's checking date after Actual Time Board
                            if (truck.GCN_DATE >= voyage.ATB)
                            {
                                //There is a truck in database and still valid
                                return TruckStatus.HaveInfoAndValid;
                            }
                        }
                        //or ETB time
                        else if (voyage.ETB.HasValue)
                        {
                            if (truck.GCN_DATE >= voyage.ETB)
                            {
                                //There is a truck in database and still valid
                                return TruckStatus.HaveInfoAndValid;
                            }
                        }
                    }
                }
                //There is no Truck in database or not valid and need to be checked again
                return TruckStatus.NotYetCheck;
            }
            catch (Exception e)
            {
                Logger.Error("IsTruckValidForVoy Error: ", e);
                throw e;
            }
        }
        ///
        /// <summary>
        /// Read File Excel
        /// </summary>
        /// <param name="file">file need to be read</param>
        /// <param name="excelId">Id file need to be read</param>
        /// <returns>File's content</returns>
        /// 
        private List<ExcelDetail> ReadFile(HttpPostedFileBase file, int excelId, int Continue = 2)
        {
            Logger.Info("ReadFile fileName=" + file.FileName + " excelId=" + excelId);
            try
            {
                ExcelPackage package = new ExcelPackage(file.InputStream);
                ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
                List<ExcelDetail> lstExcelDetail = new List<ExcelDetail>();
                int i;
                //Read info from row Continue of file
                for (var rowNumber = Continue; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
                {
                    var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                    ExcelDetail excelDetail = new ExcelDetail();
                    //Read from Cell i of row Continue of file;
                   
                    //Get each Cell of row Continue
                    for ( i=1; i<= workSheet.Dimension.End.Column;i++)
                    {                
                        //If file's column number is more than Max  
                        if (i >= (int)OrderInExcelFile.Max)
                        {
                            return null;
                        }
                        switch (i)
                        {
                            case (int)OrderInExcelFile.TruckNo:
                                excelDetail.TruckNo = RemoveSymbol(workSheet.Cells[rowNumber,i].Text);
                                break;
                            case (int)OrderInExcelFile.ChassicNo:
                                excelDetail.ChassisNo = RemoveSymbol(workSheet.Cells[rowNumber, i].Text);
                                break;
                            case (int)OrderInExcelFile.DriverName:
                                excelDetail.DriverName = RemoveSymbol(workSheet.Cells[rowNumber, i].Text);
                                break;
                            case (int)OrderInExcelFile.DriverId:
                                excelDetail.DriverId = RemoveSymbol(workSheet.Cells[rowNumber, i].Text);
                                break;
                            case (int)OrderInExcelFile.LiscenseNo:
                                excelDetail.LicenseNo = RemoveSymbol(workSheet.Cells[rowNumber, i].Text);
                                break;
                            default: break;
                        }
                    }
                    //If content's column number is not enough
                    if (workSheet.Dimension.End.Column < ((int)OrderInExcelFile.Max - 1))
                    {
                        return null;
                    }
                    //Add object read from row Continue to lstExcelDetail
                    excelDetail.FileExcelId = excelId;
                    excelDetail.TruckStatus = TruckStatus.NotYetCheck;
                    lstExcelDetail.Add(excelDetail);
                }
                return lstExcelDetail;
            }
            catch (Exception e)
            {
                Logger.Error("ReadFile Error: ", e);
                throw e;
            }
        }
        ///
        /// <summary>
        /// Remove special character
        /// </summary>
        /// <param name="str">string used for removing special chars</param>
        /// <returns></returns>
        /// 
        public static string RemoveSymbol(string str)
        {
            // find any character not between bracket
            // \w: word character (\W non-word character)
            // \d: digit character (\D non-digit character)
            // \s: white space character(\D non-white space character)
            return Regex.Replace(str, @"[^\w\d\s]", "");
        }
        // Get file excel by ID
        public FileExcel GetById(int id)
        {
            return _fileExcelRepository.GetById(id);
        }
        // Get list file excel when Voyage not departed
        public List<FileExcel> GetByVoyageNotDeparted()
        {
            List<FileExcel> list = new List<FileExcel>();
            try
            {
                var listFileExcel = _fileExcelRepository.GetAll();
                foreach (var item in listFileExcel)
                {
                    VOYAGE voyage = _oraVoyageService.GetByVoyCode(item.VoyCode);
                    if (voyage.ATD == null)
                    {
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
            return list;
        }
    }
    ///
    /// <summary>
    /// Order of data to get from file excel
    /// </summary>
    /// 
    public enum OrderInExcelFile
    {
        TruckNo = 1, //Ô 1
        ChassicNo, //Ô 2
        DriverName, //Ô 3
        DriverId, //Ô 4
        LiscenseNo, // Ô 5
        Max
    }
    ///
    /// <summary>
    /// Class used for testing
    /// </summary>
    /// 
    public class FileWrapper
    {
        public bool Exist;
        public FileWrapper()
        {
            Exist = false;
        }
        public bool Exists(string path)
        {
            Exist = File.Exists(path);
            return Exist;
        }
    }
}
