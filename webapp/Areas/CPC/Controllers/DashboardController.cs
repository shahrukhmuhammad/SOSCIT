using BaseApp.Entity;
using BaseApp.System;
using System.Web.Mvc;
using Insight.Database;

namespace WebApp.Areas.CPC.Controllers
{
    [ModuleActivator, AppAuthorize(AppPermission.All, AppPermission.CPC, AppPermission.ViewCPC)]
    //[AllowAnonymous]
    public class DashboardController : AppController
    {
        public DashboardController()
        {
            
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}