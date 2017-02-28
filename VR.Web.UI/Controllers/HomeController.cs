using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;
using System.Web.Security;
using VR.OracleProvider.OracleEntities;
using VR.OracleProvider.OracleRepository;
using VR.Service.Services;
using VR.Web.UI.CustomerAttribute;
using VR.Web.UI.Models;
using VR.Web.UI.ViewModel;

namespace VR.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [LoginCustomAdmin(2)]
        public ActionResult Index()
        {
           
            return View();
        }

     
      
    }
}