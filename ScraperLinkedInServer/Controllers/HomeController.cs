using ScraperLinkedInServer.Models.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace ScraperLinkedInServer.Controllers
{
    [RoutePrefix("api/Home")]
    public class HomeController : Controller
    {
        /// <summary>
        /// </summary>
        /// <returns>sdsd</returns>
        [Authorize(Roles = Role.Admin)]
        [HttpGet]
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
