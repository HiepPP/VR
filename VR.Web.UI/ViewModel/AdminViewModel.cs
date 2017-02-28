using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VR.Web.UI.ViewModel
{
    public class AdminViewModel
    {
       
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "WebsiteAddress_error")]
        [StringLength(250)]
        public string WebsiteAddress { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "TruckType_error")]
        [StringLength(50)]
        public string TruckType { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "NetWeight_error")]
        [StringLength(50)]
        public string NetWeight { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "MaxSeat_error")]
        [StringLength(50)]
        public string MaxSeat { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "DateCheck_error")]
        [StringLength(50)]
        public string DateCheck { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "GCNStamp_error")]
        [StringLength(50)]
        public string GCNStamp { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "WeightLimit_error")]
        [StringLength(50)]
        public string WeightLimit { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "GrossWeight_error")]
        [StringLength(50)]
        public string GrossWeight { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "TowedWeight_error")]
        [StringLength(50)]
        public string TowedWeight { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "GCNDate_error")]
        [StringLength(50)]
        public string GCNDate { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "AxleNo_error")]
        [StringLength(50)]
        public string AxleNo { get; set; }
    }
}