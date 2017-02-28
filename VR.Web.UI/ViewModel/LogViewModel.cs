using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VR.Web.UI.ViewModel
{
    public class LogViewModel
    {
        [Display(Name = "No")]
        public int No;
        [DataType(DataType.Date)]
        public DateTime ImpDate { get; set; }
        [Display(Name = "Imp User")]
        public string ImpUser { get; set; }
        [Display(Name = "Vessel")]
        public string VesselCode { get; set; }
        [Display(Name = "Customer")]
        public string CustomerName { get; set; }
        [Display(Name = "Update VR-Status")]
        public string Status { get; set; }
        [Display(Name = "Continue to Imp")]
        public string Continue { get; set; }
        [Display(Name = "View Detail")]
        public string View { get; set; }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "Row Imported/Row Count")]
        public string RowImported { get; set; }
        public bool IsDone { get; set; }
        public bool IsUpdate { get; set; }
        public int ExcelId { get; set; }
    }
}