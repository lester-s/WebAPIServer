using NLog;
using SelfHostedWebApi.BuisnessLayer;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SelfHostedWebApi.HostConfig
{
    public class ServerAuthorizationFilter : AuthorizeAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ServerStaticValues.AppRole Role { get; set; }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            logger.Debug("start authorizing");
            if (!AppHandler.Instance.Settings.IsAuthActivated)
            {
                return true;
            }

            if (Role == ServerStaticValues.AppRole.noConnectionNeeded)
            {
                return true;
            }

            var identity = actionContext.RequestContext.Principal.Identity;
            logger.Debug($"Authorizing identity: {identity.Name}");
            if (identity != null && identity.IsAuthenticated)
            {
                var basicAuth = identity as BasicAuthenticationIdentity;

                if (Role == ServerStaticValues.AppRole.Admin && basicAuth.Role == ServerStaticValues.AppRole.Admin)
                {
                    return true;
                }
                else if (Role == ServerStaticValues.AppRole.reader && (basicAuth.Role == ServerStaticValues.AppRole.Admin || basicAuth.Role == ServerStaticValues.AppRole.reader))
                {
                    return true;
                }
            }

            logger.Debug($"Fail to authorize");
            return false;
        }
    }
}