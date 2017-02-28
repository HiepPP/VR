using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VR.DAL.Model;
using VR.Infrastructure.Utilities;
using VR.Service.Services;
using VR.Web.UI.ViewModel;
using VR.Web.UI.Language;
using VR.Web.UI.CustomerAttribute;

namespace VR.Web.UI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IConfigurationService _co = new ConfigurationService();
        // GET: Admin
        // create method alert
        protected void setArlert(string messager, string type)
        {       
            TempData["AlertMessager"] = messager;
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
        [LoginCustomAdmin(1)]
        public ActionResult Index()
        {
            var user = User.Identity.Name;
            // load data to view
            AdminViewModel advm = new AdminViewModel();
            var list = _co.GetAll().FirstOrDefault();
            if (list != null)
            {
                advm.Id = list.Id;
                advm.WebsiteAddress = list.WebsiteAddress;
                advm.TruckType = list.TruckType;
                advm.NetWeight = list.NetWeight;
                advm.MaxSeat = list.MaxSeat;
                advm.DateCheck = list.DateCheck;
                advm.AxleNo = list.AxleNo;
                advm.WeightLimit = list.WeightLimit;
                advm.GrossWeight = list.GrossWeight;
                advm.TowedWeight = list.TowedWeight;
                advm.GCNDate = list.GCNDate;
                advm.GCNStamp = list.GCNStamp;
        }
            return View(advm);
        }
        [HttpPost]
        public ActionResult Index(AdminViewModel model)
        {           
            //save log infor
            Logger.Info(Resource_Config.txt_confi_succes);
            try
            {
                //create object configuration
                //set value to model
                var configuration = _co.GetAll();
                var _conf = new Configuration();
                if (ModelState.IsValid)
                {

                    _conf.Id = model.Id;
                    _conf.WebsiteAddress = model.WebsiteAddress;
                    _conf.TruckType = model.TruckType;
                    _conf.NetWeight = model.NetWeight;
                    _conf.MaxSeat = model.MaxSeat;
                    _conf.DateCheck = model.DateCheck;
                    _conf.AxleNo = model.AxleNo;
                    _conf.WeightLimit = model.WeightLimit;
                    _conf.GrossWeight = model.GrossWeight;
                    _conf.TowedWeight = model.TowedWeight;
                    _conf.GCNDate = model.GCNDate;
                    _conf.GCNStamp = model.GCNStamp;
                };
                // data value use update
                string Alert_success = Resource_Config.Alert_success;
                string Alert_error = Resource_Config.Alert_error;
                if (configuration.Count() > 0)
                {
                    var box_up = _co.Update(_conf);
                   
                    if (box_up != null)
                    {
                        setArlert(Alert_success, "success");
                    }
                    //alert for update unsuccess 
                    else
                    {
                        setArlert(Alert_error, "error");
                    }
                }
                // data unvalue use insert
                else
                {
                    var box_in = _co.Insert(_conf);
                    if (box_in != null)
                    {
                        setArlert(Alert_success, "success");
                    }
                    //alert for insert unsuccess
                    else
                    {
                        setArlert(Alert_error, "error");
                    }
                }
            }
            catch (Exception ex)
            {
                //log error
                Logger.Error(Resource_Config.txt_confi_error, ex);
            }         
            return View();
        }
    }
}