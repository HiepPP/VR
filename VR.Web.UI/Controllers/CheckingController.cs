using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using VR.DAL.Model;
using VR.Infrastructure.Utilities;
using VR.OracleProvider.OracleEntities;
using VR.OracleProvider.OracleRepository;
using VR.Service.Services;
using VR.Web.UI.Language;
using VR.Web.UI.ViewModel;
using VR.Web.UI.Language;
namespace VR.Web.UI.Controllers
{
    ///
    /// <summary>
    /// Where the client want to upload file and check validation of truck
    /// </summary>
    /// 
    [Authorize]
    public class CheckingController : Controller
    {

        private readonly IExcelDetailService _excelDetailService = new ExcelDetailService();
        //private readonly IExcelDetailService _excelDetailService;
        private readonly ICustomerService _customerService = new CustomerService();
        private readonly IFileExcelService _fileExcelService = new FileExcelService();
        private readonly IOraVoyageService _oraVoyageService = new OraVoyageService();
        private readonly IOraVoyageRepository _oraVoyageRepository = new OraVoyageRepository();
        private readonly IConfigurationService _configService = new ConfigurationService();
        private readonly IOraTruckService _oraTruckService = new OraTruckService();
        //public CheckingController(IOraTruckService _oraTruckService, IExcelDetailService _excelDetailService)
        //{
        //    this._oraTruckService = _oraTruckService;
        //    this._excelDetailService = _excelDetailService;
        //}
        CookieContainer cookieContainer;
        static readonly string VRurl = WebConfigurationManager.AppSettings["VRurl"];
        static readonly string VRCaptchaURL = WebConfigurationManager.AppSettings["VRCaptchaURL"];      
        ///
        /// <summary>
        /// Display ImportExcel View
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult ImportExcel()
        {
            Logger.Info("CheckingController ImportExcel");
            try
            {
                List<VOYAGE> lstVoyage = _oraVoyageService.GetVoyATD();
                if (lstVoyage.Count > 0)
                {
                    //Get data for dropdownlist of voyages
                    ViewBag.VesselId = new SelectList(lstVoyage, "Id", "Code");
                    ViewBag.VesselName = lstVoyage.Select(x => new List<object>() { x.ID, x.NAME });
                }
                else
                {
                    //There's still no voyage in database
                    ViewBag.VesselId = null;
                    ViewBag.VesselName = null;
                }
            }
            catch (Exception e)
            {
                Logger.Error("CheckingController ImportExcel Error: ", e);
            }
            return View();
        }
        ///
        /// <summary>
        /// Save File Excel Uploading
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportExcel(ImportExcelViewModel model)
        {
            Logger.Info("CheckingController ImportExcel(model)");
            try
            {
                if (ModelState.IsValid)
                {
                    //Get UserName
                    //string userName = User.Identity.Name;
                    string userName = User.Identity.Name; 
                    List<VOYAGE> lstVoyage = _oraVoyageService.GetVoyATD();
                    if (lstVoyage.Count > 0)
                    {
                        //Get data for dropdownlist of voyages
                        ViewBag.VesselId = new SelectList(lstVoyage, "Id", "Code");
                        ViewBag.VesselName = lstVoyage.Select(x => new List<object>() { x.ID, x.NAME });
                    }
                    var voyage = _oraVoyageRepository.GetById(model.VesselId);
                    //Import Function of FileExcelService
                    //TestUserId will be re-code after Login function
                    int[] state = _fileExcelService.Import(voyage.CODE, userName, model.CustomerId, model.FileImport);
                    //result of import
                    switch (state[0])
                    {
                        case (int)ImportedFileState.ContentBlank:
                            ModelState.AddModelError("", Resource_FileExcelUpload.ErrorContentBlank);
                            break;
                        case (int)ImportedFileState.Done:
                            return RedirectToAction("GetInfoRegis", new { excelId = state[1] });
                        case (int)ImportedFileState.Existed:
                            ModelState.AddModelError("", Resource_FileExcelUpload.ErrorExisted);
                            break;
                        case (int)ImportedFileState.NotEnoughParams:
                            ModelState.AddModelError("", Resource_FileExcelUpload.ErrorNotEnoughParams);
                            break;
                        case (int)ImportedFileState.NotFinished:
                            ModelState.AddModelError("", Resource_FileExcelUpload.ErrorNotFinished);
                            break;
                        case (int)ImportedFileState.WrongExtension:
                            ModelState.AddModelError("", Resource_FileExcelUpload.ErrorWrongExtension);
                            break;
                        case (int)ImportedFileState.ContentSizeWrong:
                            ModelState.AddModelError("", Resource_FileExcelUpload.ErrorContentSizeWrong);
                            break;
                    }
                }
                else
                {
                    ModelState.AddModelError("", Resource_FileExcelUpload.ErrorNotEnoughParams);
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", Resource_FileExcelUpload.ErrorServer);
                Logger.Error("CheckingController ImportExcel(model) Error: ", e);
            }
            return View(model);
        }
        /// <summary>
        /// Ajax get customer by voyage id
        /// </summary>
        /// <param name="voyId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCustomerByVoy(int voyId)
        {
            Logger.Info("Checking Controller GetCustomerByVoy Error: ");
            try
            {               
                //Get list of customer by voyage Id
                var lstPartner = _customerService.GetListPartnerOracle(voyId);
                var lstCustomer = lstPartner.Select(x => new List<object>() { x.ID, x.NAME });
                return Json(new { lstCustomer });
            }
            catch (Exception e)
            {
                Logger.Error("Checking Controller GetCustomerByVoy Error: ", e);
                return Json(new { });
            }
        }
        /// <summary>
        /// Get Truck Information
        /// </summary>
        /// <param name="excelId">excel detail Id</param>
        /// <returns>View</returns>
        public ActionResult GetInfoRegis(int excelId, int? excelDetailId)
        {
            try
            {
                var model = new GetRegisInfoViewModel();
                int truckOnProcessingId = 0;
                if(TempData[Resource_GetTruck.truckinfo] != null)
                {
                    var truckinfo = (TRUCK)TempData[Resource_GetTruck.truckinfo];
                    if (truckinfo.ID != 0)
                    {
                        model.Truck = truckinfo;
                    }
                }
                var lstExcelDetail = _excelDetailService.GetListByFileExcelId(excelId).ToList();
                if (lstExcelDetail.Any())
                {
                    var chuathaotac = new ExcelDetail();
                    if (excelDetailId == null)
                    {
                        chuathaotac = lstExcelDetail.Where(x => x.TruckStatus == TruckStatus.NotYetCheck).FirstOrDefault();
                    }
                    else
                    {
                        chuathaotac = lstExcelDetail.Where(x => x.Id == excelDetailId).FirstOrDefault();
                    }            
                    if (chuathaotac != null)
                    {
                        ConnectToVR();
                        model.OnProcessing = true;
                        model.ExcelDetailId = chuathaotac.Id;
                        model.FileExcelId = chuathaotac.FileExcelId;
                        if (chuathaotac.TruckNo!=null)
                        {
                            model.LicensePlate = chuathaotac.TruckNo;
                        }
                        else
                        {
                            model.LicensePlate = chuathaotac.ChassisNo;
                        }
                        truckOnProcessingId = chuathaotac.Id;
                        model.CaptchaURL = TempData[Resource_GetTruck.captchaURL].ToString();
                        model.HaveNotCheckYet = true;
                    }
                    else
                    {
                        var randomtruck = lstExcelDetail.FirstOrDefault();
                        truckOnProcessingId = randomtruck.Id;
                        if (randomtruck.TruckNo != null)
                        {
                            model.Truck = _oraTruckService.GetByTruckNo(randomtruck.TruckNo);
                        }
                        else
                        {
                            model.Truck = _oraTruckService.GetByTruckNo(randomtruck.ChassisNo);
                        }
                        model.LicensePlate = randomtruck.TruckNo;
                        //model.Status = TruckStatus.OnProcessing;
                        model.OnProcessing = true;
                        model.HaveNotCheckYet = false;
                    }
                    lstExcelDetail.ForEach(x =>
                    {
                        var newPlate = new PlateViewModel
                        {
                            PlateStatus = x.TruckStatus,
                            ExcelDetailId = x.Id,
                        };
                        if (x.Id == truckOnProcessingId)
                        {
                            newPlate.OnProcessing = true;
                        }
                        if (x.TruckNo != null)
                        {
                            newPlate.PlateNo = x.TruckNo;
                        }
                        else
                        {
                            newPlate.PlateNo = x.ChassisNo;
                        }
                        model.PlateLst.Add(newPlate);
                    });
                    var getFileExceldata = _fileExcelService.GetById(lstExcelDetail.FirstOrDefault().FileExcelId);
                    model.Customer = _customerService.GetById(getFileExceldata.CustomerId.Value).NAME;
                    //model.Customer = "Green Feed"; //for testing
                    model.VesselName = _oraVoyageService.GetByVoyCode(getFileExceldata.VoyCode).NAME;
                    //model.VesselName = "Thong Nhat 1"; //for testing
                    if(TempData[Resource_GetTruck.errorWhenSubmit] != null)
                    {
                        TempData[Resource_GetTruck.message] = TempData[Resource_GetTruck.errorWhenSubmit].ToString();
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Info("GetInfoRegis:", ex);
                return View(); //for testing
            }          
        }
        public ActionResult GetDataFromVR(string CaptchaConfirm, string LicensePlate, int ExcelDetailId, int FileExcelId)
        {
            try
            {
                //LicensePlate = "29A01234T"; //for test
                var html = (HtmlClassModel)Session["html"];
                var outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
                outgoingQueryString.Add(Resource_GetTruck.VIEWSTATE, html.__VIEWSTATE);
                outgoingQueryString.Add(Resource_GetTruck.VIEWSTATEGENERATOR, html.__VIEWSTATEGENERATOR);
                outgoingQueryString.Add(Resource_GetTruck.EVENTVALIDATION, html.__EVENTVALIDATION);
                outgoingQueryString.Add(Resource_GetTruck.BienDK, LicensePlate);
                outgoingQueryString.Add(Resource_GetTruck.Captcha, CaptchaConfirm);
                outgoingQueryString.Add(Resource_GetTruck.Button1, html.Button1);
                string postData = outgoingQueryString.ToString();
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(VRurl);
                webRequest.CookieContainer = (CookieContainer)Session["cookie"];
                //webRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                //1 loi 500
                //StreamWriter reqStream = new StreamWriter(webRequest.GetRequestStream());
                //reqStream.Write(postData);
                //byte[] postArray = Encoding.ASCII.GetBytes(postData);
                //Stream newStream = webRequest.GetRequestStream();
                //newStream.Write(postArray, 0, postArray.Length);
                //reqStream.Close();
                //StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream());
                //string Result = sr.ReadToEnd();
                //2
                StreamWriter reqStream = new StreamWriter(webRequest.GetRequestStream());
                reqStream.Write(postData);
                reqStream.Close();
                StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream());
                string Result = sr.ReadToEnd();
                var truckinfo = GetValueVR(Result, LicensePlate, ExcelDetailId);
                if(truckinfo != null)
                {
                    TempData[Resource_GetTruck.message] = "Lấy thông tin thành công";
                    TempData[Resource_GetTruck.truckinfo] = truckinfo;
                    return RedirectToAction("GetInfoRegis",new { excelId = FileExcelId });
                }
                else
                {
                    TempData[Resource_GetTruck.message] = TempData[Resource_GetTruck.errorWhenSubmit].ToString();
                    return RedirectToAction("GetInfoRegis", new { excelId = FileExcelId, ExcelDetailId = ExcelDetailId });
                }
                return View();
            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    string text = reader.ReadToEnd();
                }
                return View();
            }

        }
        /// <summary>
        /// Create request to VR.ORG, get captcha link
        /// </summary>
        public void ConnectToVR()
        {
            string siteContent = string.Empty;

            IWebProxy webProxy = WebRequest.DefaultWebProxy;
            webProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(VRurl);
            request.Proxy = webProxy;
            if (cookieContainer == null)
            {
                cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                Session["cookie"] = cookieContainer;
            }
            var html = new HtmlClassModel();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //For cookie testing
            int cookie = cookieContainer.Count;
            foreach (Cookie c in cookieContainer.GetCookies(request.RequestUri))
            {
                var r = ("Cookie['" + c.Name + "']: " + c.Value);
            }
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (Stream responseStream = response.GetResponseStream())               // Load the response stream
                using (StreamReader streamReader = new StreamReader(responseStream))       // Load the stream reader to read the response
                {
                    siteContent = streamReader.ReadToEnd();
                    html = HtmlAgility(siteContent);
                    Session["html"] = html;
                }
            }
            TempData[Resource_GetTruck.captchaURL] = VRCaptchaURL + html.img;
        }
        /// <summary>
        /// Get viewstate from html by the id of the tag.
        /// </summary>
        /// <param name="siteContent">html result</param>
        /// <returns>HtmlClassModel</returns>
        public HtmlClassModel HtmlAgility(string siteContent)
        {
            var htmlclass = new HtmlClassModel();
            var doc = new HtmlDocument();
            doc.LoadHtml(siteContent);

            foreach (var item in doc.DocumentNode.SelectNodes("//input").ToList())
            {
                HtmlAttribute att = item.Attributes["value"];
                if (att != null)
                {
                    if (att.OwnerNode.Id == "__VIEWSTATE")
                    {
                        htmlclass.__VIEWSTATE = att.Value;
                    }
                    else if (att.OwnerNode.Id == "__VIEWSTATEGENERATOR")
                    {
                        htmlclass.__VIEWSTATEGENERATOR = att.Value;
                    }
                    else if (att.OwnerNode.Id == "__EVENTVALIDATION")
                    {
                        htmlclass.__EVENTVALIDATION = att.Value;
                    }
                    else if (att.OwnerNode.Id == "Button1")
                    {
                        htmlclass.Button1 = att.Value;
                    }
                }
            }
            foreach (var imgItem in doc.DocumentNode.SelectNodes("//img").ToList())
            {
                HtmlAttribute att = imgItem.Attributes["src"];
                if (att != null)
                {
                    if (att.OwnerNode.Id == "Image1")
                    {
                        htmlclass.img = att.Value;
                        break;
                    }
                }
            }
            return htmlclass;
        }
        /// <summary>
        /// Get data from html result after submit captcha. Saving data
        /// </summary>
        /// <param name="htmlResponse">html Response</param>
        /// <param name="LicensePlate">LicensePlate</param>
        /// <param name="ExcelDetailId">Excel Detail Id</param>
        /// <returns></returns>
        public TRUCK GetValueVR(string htmlResponse, string LicensePlate, int ExcelDetailId)
        {
            try
            {
                var doc = new HtmlDocument();
                var truck = new TRUCK();
                doc.LoadHtml(htmlResponse);
                var error = GetSpanContent("lblErrMsg", doc);
                if ((!error.Equals("Sai mã xác nhận!")) && (!error.Equals("Không tìm thấy thông tin phương tiện này.")))
                {
                    var findConfigdata = _configService.GetById(1);
                    if (findConfigdata != null)
                    {
                        var findtruck = _oraTruckService.GetByTruckNo(LicensePlate);

                        var trucktype = GetSpanContent(findConfigdata.TruckType, doc);
                        if (trucktype != null && trucktype.Contains("Ô tô con"))
                        {
                            trucktype = "T";
                        }
                        else if (trucktype != null && trucktype.Contains("đầu kéo"))
                        {
                            trucktype = "V";
                        }
                        else if (trucktype != null && trucktype.Contains("sơ mi"))
                        {
                            trucktype = "R";
                        }
                        if (findtruck != null)
                        {
                            findtruck.TRK_TYPE = trucktype;
                            findtruck.NET_WEI_LIMIT = GetSpanContent(findConfigdata.NetWeight, doc).GetNumberInString();
                            findtruck.KERB_MASS = GetSpanContent(findConfigdata.MaxSeat.ToString(), doc).GetNumberInString();
                            findtruck.TRK_AXLE = GetSpanContent(findConfigdata.AxleNo, doc).GetNumberInString();
                            findtruck.DATE_CHECK = GetSpanContent(findConfigdata.DateCheck, doc).f_CDate();
                            findtruck.GCN_STAMP = GetSpanContent(findConfigdata.GCNStamp, doc);
                            findtruck.GCN_DATE = GetSpanContent(findConfigdata.GCNDate.ToString(), doc).f_CDate();
                            findtruck.WEI_LIMIT = GetSpanContent(findConfigdata.WeightLimit, doc).GetNumberInString();
                            findtruck.CROSS_WEI_LIMIT = GetSpanContent(findConfigdata.GrossWeight, doc).GetNumberInString();
                            findtruck.TOWED_WEIGHTED = GetSpanContent(findConfigdata.TowedWeight, doc).GetNumberInString();
                            findtruck.TRK_NO = LicensePlate;
                            if (_oraTruckService.Update(findtruck) != null)
                            {
                                var excelupdate = _excelDetailService.GetById(ExcelDetailId);
                                excelupdate.TruckStatus = TruckStatus.GetInfoAndValid;
                                _excelDetailService.Update(excelupdate);
                                return findtruck;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            truck.TRK_TYPE = trucktype;
                            truck.NET_WEI_LIMIT = GetSpanContent(findConfigdata.NetWeight, doc).GetNumberInString();
                            truck.KERB_MASS = GetSpanContent(findConfigdata.MaxSeat.ToString(), doc).GetNumberInString();
                            truck.TRK_AXLE = GetSpanContent(findConfigdata.AxleNo, doc).GetNumberInString();
                            truck.DATE_CHECK = GetSpanContent(findConfigdata.DateCheck, doc).f_CDate();
                            truck.GCN_STAMP = GetSpanContent(findConfigdata.GCNStamp, doc);
                            truck.GCN_DATE = GetSpanContent(findConfigdata.GCNDate.ToString(), doc).f_CDate();
                            truck.WEI_LIMIT = GetSpanContent(findConfigdata.WeightLimit, doc).GetNumberInString();
                            truck.CROSS_WEI_LIMIT = GetSpanContent(findConfigdata.GrossWeight, doc).GetNumberInString();
                            truck.TOWED_WEIGHTED = GetSpanContent(findConfigdata.TowedWeight, doc).GetNumberInString();
                            truck.TRK_NO = LicensePlate;
                            if (_oraTruckService.Insert(truck) != null)
                            {
                                var excelupdate = _excelDetailService.GetById(ExcelDetailId);
                                excelupdate.TruckStatus = TruckStatus.GetInfoAndValid;
                                _excelDetailService.Update(excelupdate);
                                return truck;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
                TempData[Resource_GetTruck.errorWhenSubmit] = error;
                return null;
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message);
                return null;
            }
            
        }
        /// <summary>
        /// submethod of GetValueVR, extract data from span tag
        /// </summary>
        /// <param name="valueconfig">value of id of span tag</param>
        /// <param name="doc">HtmlDocument</param>
        /// <returns>TRUCK</returns>
        public string GetSpanContent(string valueconfig, HtmlDocument doc)
        {
            var node = doc.DocumentNode.SelectSingleNode("//span[@id='" + valueconfig + "']");
            var innerText = string.Empty;
            if (node != null)
            {
                innerText = node.InnerText;
            }
            return innerText;
        }
        /// <summary>
        /// Get truck info by return json to view
        /// </summary>
        /// <param name="exceldetailId">excel detail Id</param>
        /// <returns>string</returns>
        public ActionResult GetJsonPlateInfo(int exceldetailId)
        {
            var exceldetailinfo = _excelDetailService.GetById(exceldetailId);
            if(exceldetailinfo != null)
            {
                if (exceldetailinfo.TruckStatus == TruckStatus.NotYetCheck)
                {
                    var exceldetailJson = new JsonTruckInfo();
                    exceldetailJson.Status = TruckStatus.NotYetCheck.ToString();
                    exceldetailJson.ExcelDetailId = exceldetailId;
                    if (exceldetailinfo.TruckNo != null)
                    {
                        exceldetailJson.PlateNumber = exceldetailinfo.TruckNo;
                    }
                    else
                    {
                        exceldetailJson.PlateNumber = exceldetailinfo.ChassisNo;
                    }
                    ConnectToVR();
                    exceldetailJson.ImgCaptcha = TempData[Resource_GetTruck.captchaURL].ToString();
                    return Json(exceldetailJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var getTruck = new TRUCK();
                    if (exceldetailinfo.TruckNo != null)
                    {
                        getTruck = _oraTruckService.GetByTruckNo(exceldetailinfo.TruckNo);
                    }
                    else
                    {
                        getTruck = _oraTruckService.GetByTruckNo(exceldetailinfo.ChassisNo);
                    }
                    return Json(getTruck, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new {message = "Có lỗi xảy ra", JsonRequestBehavior.AllowGet});
        }
    }
}