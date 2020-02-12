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
    }
}