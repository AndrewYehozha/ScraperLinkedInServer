using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;

namespace ScraperLinkedInServer.Utilities
{
    public class ScraperLinkedInApiController : ApiController
    {
        protected IIdentity Identity
        {
            get
            {
                return Thread.CurrentPrincipal.Identity;
            }
        }

        protected new IHttpActionResult Json(object data)
        {
            return Ok(data); ;
        }
    }
}