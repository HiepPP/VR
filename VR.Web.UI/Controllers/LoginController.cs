using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using System.Web.Security;
using VR.OracleProvider.OracleEntities;
using VR.OracleProvider.OracleRepository;
using VR.Service.Services;
using VR.Web.UI.Models;
using VR.Web.UI.ViewModel;
using VR.Web.UI.CustomerAttribute;
using VR.Web.UI.Language;
using VR.Infrastructure.Utilities;
using System.Web;
using System;

namespace VR.Web.UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IOraAppUserService _co = new OraAppUserService();
        private readonly IUserService _userService = new UserService();
        IOraBillRepository _asdasd = new OraBillRepository();
        OraAppUserRepository oracleuser = new OraAppUserRepository();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginViewModel taikhoan)
        {
            //check not null
            if (ModelState.IsValid)
            {
                //check usename
                var checkLogin = _co.Check(taikhoan.USER_NAME, taikhoan.PWD);
                if (taikhoan.USER_NAME != null && taikhoan.PWD != null)
                {
                    if (checkLogin == null)
                        TempData["message"] = Resource_LoginHome.User_error;
                }
                //check login = true
                //check role login
                if (checkLogin != null)
                {
                    //save log
                    Logger.Info(Resource_LoginHome.Login_success);
                    FormsAuthentication.SetAuthCookie(taikhoan.USER_NAME, true);
                    Session[Resource_LoginHome.Session_user] = checkLogin.USER_NAME;
                    //check role of username
                    string username = checkLogin.USER_NAME;
                    int numRole = oracleuser.CheckRole(username);
                    if (numRole == 1)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (numRole == 2)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    //login fail
                    Logger.Error(Resource_LoginHome.Login_fail);
                }
            };
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            return RedirectToAction("Index");
        }
    }
}