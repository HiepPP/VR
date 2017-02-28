using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VR.Service.Services;
using VR.Web.UI.Language;

namespace VR.Web.UI.CustomerAttribute
{
    public class LoginCustomAdminAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly IOraAppUserService _appUserService = new OraAppUserService();        
        public LoginCustomAdminAttribute(int role)
        {
            this.Role = role;
        }
        //check role to redirect action
        public int Role
        { get; set; }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //get curent user to check role
            var username = HttpContext.Current.User.Identity.Name;
            var checkrole = _appUserService.CheckRole(username.ToString());
            if (Role != checkrole)
            {
                //redirect to error http
                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}