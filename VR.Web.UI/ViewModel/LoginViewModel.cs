using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VR.Web.UI.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "USER_NAME_error")]
        [StringLength(60)]
        public string USER_NAME { get; set; }
        [Required(ErrorMessageResourceType = typeof(Language.Resource_Config), ErrorMessageResourceName = "PWD_error")]
        [DataType(DataType.Password)]
        public string PWD { get; set; }
    }
}