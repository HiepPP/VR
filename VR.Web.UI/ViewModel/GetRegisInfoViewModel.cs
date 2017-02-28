using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VR.DAL.Model;
using VR.OracleProvider.OracleEntities;

namespace VR.Web.UI.ViewModel
{
    public class GetRegisInfoViewModel
    {
        public GetRegisInfoViewModel()
        {
            PlateLst = new List<PlateViewModel>();
        }
        public string VesselCode { get; set; }
        [Display(Name = "Vessel Name")]
        public string VesselName { get; set; }
        [Display(Name = "Customer")]
        public string Customer { get; set; }
        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; }
        [Display(Name = "Capcha")]
        public string CaptchaConfirm { get; set; }
        public TruckStatus Status { get; set; }
        public string CaptchaURL { get; set; }
        public List<PlateViewModel> PlateLst { get; set; }
        public int truckId { get; set; }
        public int ExcelDetailId { get; set; }
        public bool HaveNotCheckYet { get; set; }
        public TRUCK Truck { get; set; }
        public int FileExcelId { get; set; }
        public bool OnProcessing { get; set; }
    }
}