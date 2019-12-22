using BaseApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [AppAuthorize(AppPermission.All, AppPermission.Bank, AppPermission.ViewBank)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}