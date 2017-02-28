using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VR.Web.UI.ViewModel
{
    public class ListResultViewModel
    {
        public ListResultViewModel()
        {
            ResultViewModels = new List<ResultViewModel>();
        }

        [Display(Name = "Vessel Code")]
        public string VesselCode { get; set; }
        [Display(Name = "Vessel Name")]
        public string VesselName { get; set; }
        [Display(Name = "Customer")]
        public string Customer { get; set; }
        
        [Display(Name = "FileName")]
        public string FileName { get; set; }
        public ICollection<ResultViewModel> ResultViewModels { get; set; }
    }
}

