using System.Web.Http;
using System.Web.Http.Controllers;

namespace SelfHostedWebApi.HostConfig
{
    public class ServerAuthorizationFilter : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var identity = actionContext.RequestContext.Principal.Identity;
            //if (identity == null && HttpContext.Current != null)
            //    identity = HttpContext.Current.User.Identity;

            if (identity != null && identity.IsAuthenticated)
            {
                var basicAuth = identity as BasicAuthenticationIdentity;

                // do your business validation as needed
                //var user = new BusUser();
                //if (user.Authenticate(basicAuth.Name, basicAuth.Password))
                return true;
            }

            return false;
        }
    }
}