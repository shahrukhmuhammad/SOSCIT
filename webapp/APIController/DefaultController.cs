using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace WebApp.APIController
{
    public class DefaultController : ApiController
    {
        // GET: Default
        [System.Web.Http.HttpGet]
        public string Index()
        {
            return "Hi";
        }
    }
}