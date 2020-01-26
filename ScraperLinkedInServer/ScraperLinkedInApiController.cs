using System.Security.Principal;
using System.Threading;
using System.Web.Http;

namespace ScraperLinkedInServer
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

        /// <summary></summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected new IHttpActionResult Json(object data)
        {
            return Ok(data); ;
        }
    }
}