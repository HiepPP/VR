using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VR.Web.UI.ViewModel
{
    public class ImportExcelViewModel
    {
        [Required]
        [Display(Name ="Vessel Code")]
        public int VesselId { get; set; }
 
        [Display(Name = "Vessel Name")]
        public string VesselName { get;set;}        
        [Required]        
        public int CustomerId { get; set; }
        [Display(Name = "Customer")]
        public int CustomerName { get; set; }
        [DataType(DataType.Upload)]
        public HttpPostedFileBase FileImport { get; set; }
    }
}