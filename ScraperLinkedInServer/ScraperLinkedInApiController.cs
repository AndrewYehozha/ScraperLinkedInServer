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
        protected new IHttpActionResult JsonSuccess(object data)
        {
            return Ok(new
            {
                success = true,
                response = data
            });
        }

        /// <summary></summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected new IHttpActionResult JsonError(object data)
        {
            return Ok(new
            {
                success = false,
                response = data
            });
        }
    }
}