using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VR.DAL.Model;

namespace VR.Web.UI.ViewModel
{
    public class PlateViewModel
    {
        public string PlateNo { get; set; }
        public TruckStatus? PlateStatus { get; set; }
        public int ExcelDetailId { get; set; }
        public bool OnProcessing { get; set; }
    }
    public class JsonTruckInfo
    {
        public string PlateNumber { get; set; }
        public string  ImgCaptcha { get; set; }
        public int ExcelDetailId { get; set; }
        public string Status { get; set; }       
    }
}