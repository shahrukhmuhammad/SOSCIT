using BaseApp.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Hubs;
using Insight.Database;

namespace WebApp.Areas.Dashboard.Controllers
{
    [AppAuthorize]
    public class DashboardController : AppController
    {
        RealTimeHub realtime = new RealTimeHub();

        //private ICmsPage webPage;
        //private ICmsContent webContent;
        //private ICmsSlide webSlide;
        //private ICmsNews webNews;
        //private ICmsFile webFile;

        public DashboardController()
        {
            //webPage = db.As<ICmsPage>();
            //webContent = db.As<ICmsContent>();
            //webSlide = db.As<ICmsSlide>();
            //webNews = db.As<ICmsNews>();
            //webFile = db.As<ICmsFile>();
        }

        public ActionResult Index()
        {
            #region CMS
            //ViewBag.RecentPages = 0;
            //ViewBag.RecentSlides = 0;
            //ViewBag.RecentContents = 0;
            //ViewBag.RecentNews = 0;
            //ViewBag.RecentFiles = 0;
            #endregion

            return View();
        }
    }
}