using System.Web.Mvc;

namespace WebApp.Areas.CPC
{
    public class CPCAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CPC";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CPC_default",
                "CPC/{controller}/{action}/{id}",
                new { controller= "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}